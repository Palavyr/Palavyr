using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;


namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3Deleter
    {
        Task<bool> DeleteObjectFromS3Async(string bucket, string fileKey);
        Task<bool> DeleteObjectsFromS3Async(string bucket, string[] fileKeys);
    }

    public class S3Deleter : IS3Deleter
    {
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<S3Deleter> logger;

        public S3Deleter(IAmazonS3 s3Client, ILogger<S3Deleter> logger)
        {
            this.s3Client = s3Client;
            this.logger = logger;
        }

        public async Task<bool> DeleteObjectFromS3Async(string bucket, string fileKey)
        {
            logger.LogDebug($"Deleting {fileKey} from {bucket} on S3");

            var deleteRequest = new DeleteObjectRequest()
            {
                BucketName = bucket,
                Key = fileKey
            };

            try
            {
                var response = await s3Client.DeleteObjectAsync(deleteRequest);
                logger.LogInformation($"Response: {response}");
                logger.LogInformation($"Deleted {fileKey} from {bucket}");

                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Failed to delete files " + ex.Message);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteObjectsFromS3Async(string bucket, string[] fileKeys)
        {
            logger.LogDebug($"Deleting {string.Join(", ", "fileKeys")} from {bucket} on S3");

            var keys = new List<KeyVersion>();
            foreach (var key in fileKeys)
            {
                keys.Add(new KeyVersion() {Key = key});
            }

            var deleteRequest = new DeleteObjectsRequest()
            {
                BucketName = bucket,
                Objects = keys
            };

            try
            {
                var response = await s3Client.DeleteObjectsAsync(deleteRequest);
                logger.LogInformation($"Response: {response}");
                logger.LogInformation($"Deleted {string.Join(", ", fileKeys)} from {bucket}");

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