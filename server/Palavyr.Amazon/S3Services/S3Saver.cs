using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.AmazonServices;

namespace Palavyr.Amazon.S3Services
{
    public interface IS3Saver
    {
        Task<bool> SaveZipToS3(string zipPath, string snapshotTimeStamp, string snapshotName);
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

        public async Task<bool> SaveZipToS3(string zipPath, string snapshotTimeStamp, string snapshotName)
        {
            
            var fileKey = Path.Combine(AmazonConstants.SnapshotsDir, snapshotTimeStamp, snapshotName).Replace("\\", "/");
            var putRequest = new PutObjectRequest()
            {
                BucketName = AmazonConstants.ArchivesBucket,
                FilePath = zipPath,
                Key = fileKey
            };
            try
            {
                var response = await s3Client.PutObjectAsync(putRequest);
                logger.LogInformation($"Response: {response}");
                logger.LogInformation($"Saved {zipPath} to {fileKey} in {AmazonConstants.ArchivesBucket}");
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