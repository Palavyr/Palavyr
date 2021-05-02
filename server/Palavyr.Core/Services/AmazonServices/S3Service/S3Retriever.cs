#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3Retriever
    {
        Task<bool> GetLatestDatabaseBackup(string bucket, string s3FileKey, string saveToPath);
        Task<string[]?> DownloadObjectsFromS3(string bucket, List<AttachmentMeta> metas, CancellationToken cancellationToken);
    }

    public class S3Retriever : IS3Retriever
    {
        private readonly ITempPathCreator tempPathCreator;
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<IS3Retriever> logger;

        public S3Retriever(
            ITempPathCreator tempPathCreator,
            IAmazonS3 s3Client,
            ILogger<IS3Retriever> logger
        )
        {
            this.tempPathCreator = tempPathCreator;
            this.s3Client = s3Client;
            this.logger = logger;
        }

        public async Task<string[]?> DownloadObjectsFromS3(string bucket, List<AttachmentMeta> metas, CancellationToken cancellationToken)
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
                var localTempPaths = new List<string>();

                for (var i = 0; i < responses.Length; i++)
                {
                    var response = responses[i];
                    var riskyName = metas[i].RiskyName;

                    var localTempPath = tempPathCreator.Create(string.Join("-", new[] {GuidUtils.CreateShortenedGuid(1), riskyName}));
                    writesAreComplete.Add(response.WriteResponseStreamToFileAsync(localTempPath, false, cancellationToken));
                    localTempPaths.Add(localTempPath);
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

        private string ExtractFileNameFromKey(string key)
        {
            return key.Split(Delimiters.UnixDelimiter).Last();
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