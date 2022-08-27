
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Configuration;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.TemporaryPaths;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3Downloader
    {
        Task<IHaveBeenDownloadedFromCloudToLocal[]?> DownloadObjectsFromS3(List<CloudFileDownloadRequest> metas, CancellationToken cancellationToken);
        Task<bool> CheckIfFileExists(string bucket, string key);
    }

    public class S3Downloader : IS3Downloader
    {
        private readonly ITemporaryPath temporaryPath;
        private readonly IAmazonS3 s3Client;
        private readonly ConfigContainerServer config;
        private readonly ILogger<IS3Downloader> logger;

        private string UserDataBucket => config.AwsUserDataBucket;

        public S3Downloader(
            ITemporaryPath temporaryPath,
            IAmazonS3 s3Client,
            ConfigContainerServer config,
            ILogger<S3Downloader> logger
        )
        {
            this.temporaryPath = temporaryPath;
            this.s3Client = s3Client;
            this.config = config;
            this.logger = logger;
        }

        public async Task<IHaveBeenDownloadedFromCloudToLocal[]?> DownloadObjectsFromS3(List<CloudFileDownloadRequest> metas, CancellationToken cancellationToken)
        {
            var areComplete = new List<Task<GetObjectResponse>>();
            foreach (var meta in metas)
            {
                var objectRequest = new GetObjectRequest()
                {
                    BucketName = UserDataBucket,
                    Key = meta.LocationKey
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
                var localTempPaths = new List<IHaveBeenDownloadedFromCloudToLocal>();

                for (var i = 0; i < responses.Length; i++)
                {
                    var response = responses[i];
                    var riskyName = metas[i].FileNameWithExtension;

                    var s3DownloadFile = temporaryPath.CreateLocalS3SavePath(riskyName);
                    writesAreComplete.Add(response.WriteResponseStreamToFileAsync(s3DownloadFile.TempFilePath, false, cancellationToken));
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

        public async Task<bool> CheckIfFileExists(string bucket, string key)
        {
            try
            {
                await s3Client.GetObjectMetadataAsync(
                    new GetObjectMetadataRequest()
                    {
                        BucketName = bucket,
                        Key = key
                    });

                return true;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw;
            }
        }
    }
}