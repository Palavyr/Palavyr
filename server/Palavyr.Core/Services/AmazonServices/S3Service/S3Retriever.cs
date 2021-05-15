#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.TemporaryPaths;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3Retriever
    {
        Task<bool> GetLatestDatabaseBackup(string bucket, string s3FileKey, string saveToPath);
        Task<S3DownloadFile[]?> DownloadObjectsFromS3(string bucket, List<AttachmentMeta> metas, CancellationToken cancellationToken);
    }

    public class S3Retriever : IS3Retriever
    {
        private readonly ITemporaryPath temporaryPath;
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<IS3Retriever> logger;

        public S3Retriever(
            ITemporaryPath temporaryPath,
            IAmazonS3 s3Client,
            ILogger<IS3Retriever> logger
        )
        {
            this.temporaryPath = temporaryPath;
            this.s3Client = s3Client;
            this.logger = logger;
        }

        public async Task<S3DownloadFile[]?> DownloadObjectsFromS3(string bucket, List<AttachmentMeta> metas, CancellationToken cancellationToken)
        {
            var areComplete = new List<Task<GetObjectResponse>>();
            foreach (var meta in metas)
            {
                var objectRequest = new GetObjectRequest()
                {
                    BucketName = bucket,
                    Key = meta.S3Key
                };
                areComplete.Add(s3Client.GetObjectAsync(objectRequest));
            }

            GetObjectResponse[] responses;
            try
            {
                responses = await Task.WhenAll(areComplete);
            }
            catch (AmazonS3Exception ex)
            {
                logger.LogDebug($"Unable to download from s3: {ex.Message}");
                return null;
            }

            try
            {
                var writesAreComplete = new List<Task>();
                var localTempPaths = new List<S3DownloadFile>();

                for (var i = 0; i < responses.Length; i++)
                {
                    var response = responses[i];
                    var riskyName = metas[i].RiskyName;

                    var s3DownloadFile = temporaryPath.CreateLocalS3SavePath(riskyName);
                    writesAreComplete.Add(response.WriteResponseStreamToFileAsync(s3DownloadFile.FullPath, false, cancellationToken));
                    localTempPaths.Add(s3DownloadFile);
                }

                await Task.WhenAll(writesAreComplete);
                logger.LogDebug($"Successfully downloaded from S3: {responses.Select(x => x.Key)}");
                return localTempPaths.ToArray();
            }
            catch (Exception ex)
            {
                logger.LogDebug("Unable to write s3 data to temp files on the server!");
                logger.LogDebug($"{ex.Message}");
                return null;
            }
        }
        

        public async Task<bool> GetLatestDatabaseBackup(string bucket, string s3FileKey, string saveToPath)
        {
            if (s3FileKey.Contains(@"\")) throw new Exception($"S3 file paths cannot have backslash: {s3FileKey}");

            // s3FileKey should be full s3 path
            var getRequest = new GetObjectRequest()
            {
                BucketName = bucket,
                Key = s3FileKey,
            };

            try
            {
                var response = await s3Client.GetObjectAsync(getRequest);
                await response.WriteResponseStreamToFileAsync(saveToPath, false, CancellationToken.None);
                logger.LogInformation($"Response: {response}");
                logger.LogInformation($"Retrieved {s3FileKey} from {bucket}");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Failed to write snapshot files: " + ex.Message);
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}