using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Sessions;


namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3FileDeleter
    {
        Task<bool> DeleteObjectFromS3Async(string bucket, string locationKey);
        Task<bool> DeleteObjectsFromS3Async(string bucket, IEnumerable<string> fileKeys);
    }

    public class Is3FileDeleter : IS3FileDeleter
    {
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<Is3FileDeleter> logger;
        private readonly ICancellationTokenTransport cancellationTokenTransport;

        public Is3FileDeleter(IAmazonS3 s3Client, ILogger<Is3FileDeleter> logger, ICancellationTokenTransport cancellationTokenTransport)
        {
            this.s3Client = s3Client;
            this.logger = logger;
            this.cancellationTokenTransport = cancellationTokenTransport;
        }

        public async Task<bool> DeleteObjectFromS3Async(string bucket, string locationKey)
        {
            logger.LogDebug($"Deleting {locationKey} from {bucket} on S3");

            var deleteRequest = new DeleteObjectRequest()
            {
                BucketName = bucket,
                Key = locationKey
            };

            try
            {
                var response = await s3Client.DeleteObjectAsync(deleteRequest, cancellationTokenTransport.CancellationToken);
                logger.LogInformation($"Response: {response}");
                logger.LogInformation($"Deleted {locationKey} from {bucket}");

                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Failed to delete files " + ex.Message);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteObjectsFromS3Async(string bucket, IEnumerable<string> fileKeys)
        {
            logger.LogDebug($"Deleting {string.Join(", ", "fileKeys")} from {bucket} on S3");

            var keys = new List<KeyVersion>();
            foreach (var key in fileKeys)
            {
                keys.Add(new KeyVersion() { Key = key });
            }

            var deleteRequest = new DeleteObjectsRequest()
            {
                BucketName = bucket,
                Objects = keys
            };

            try
            {
                var response = await s3Client.DeleteObjectsAsync(deleteRequest, cancellationTokenTransport.CancellationToken);
                logger.LogInformation($"Response: {response}");
                logger.LogInformation($"Deleted {string.Join(", ", fileKeys.ToArray())} from {bucket}");

                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Failed to delete files: " + ex.Message);
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}