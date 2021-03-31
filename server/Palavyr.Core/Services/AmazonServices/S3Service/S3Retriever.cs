using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3Retriever
    {
        Task<bool> GetLatestDatabaseBackup(string bucket, string s3FileKey, string saveToPath);
    }

    public class S3Retriever : IS3Retriever
    {
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<IS3Retriever> logger;

        public S3Retriever(IAmazonS3 s3Client, ILogger<IS3Retriever> logger)
        {
            this.s3Client = s3Client;
            this.logger = logger;
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