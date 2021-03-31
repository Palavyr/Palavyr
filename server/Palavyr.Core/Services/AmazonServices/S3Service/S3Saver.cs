using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3Saver
    {
        Task<bool> SaveZipToS3(string bucket, string localFilePath, string fileKey);
    }

    public class S3Saver : IS3Saver
    {
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<S3Saver> logger;

        public S3Saver(IAmazonS3 s3Client, ILogger<S3Saver> logger)
        {
            this.s3Client = s3Client;
            this.logger = logger;
        }

        public async Task<bool> SaveZipToS3(string bucket, string localFilePath, string fileKey)
        {
            
            var putRequest = new PutObjectRequest()
            {
                BucketName = bucket,
                FilePath = localFilePath,
                Key = fileKey
            };
            try
            {
                var response = await s3Client.PutObjectAsync(putRequest);
                logger.LogInformation($"Response: {response}");
                logger.LogInformation($"Saved {localFilePath} to {fileKey} in {bucket}");
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