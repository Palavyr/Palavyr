using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.AmazonServices;

namespace Palavyr.Amazon.S3Services
{
    public interface IS3Retriever
    {
        Task<bool> GetLatestDatabaseBackup(string s3FileKey, string saveToPath);
    }

    public class S3Retriever : IS3Retriever
    {
        private readonly IAmazonS3 s3Client;
        private readonly ILogger logger;
        
        public S3Retriever(IAmazonS3 s3Client, ILogger<S3Retriever> logger)
        {
            this.s3Client = s3Client;
            this.logger = logger ?? new Logger<S3Retriever>(new LoggerFactory());
        }

        public S3Retriever(IAmazonS3 s3Client, ILogger logger)
        {
            this.s3Client = s3Client;
            this.logger = logger;
        }

        public S3Retriever(IAmazonS3 s3Client)
        {
            this.s3Client = s3Client;
            this.logger = logger ?? new Logger<S3Retriever>(new LoggerFactory());
        }

        public async Task<bool> GetLatestDatabaseBackup(string s3FileKey, string saveToPath)
        {
            if (s3FileKey.Contains(@"\")) throw new Exception($"S3 file paths cannot have backslash: {s3FileKey}");

            // s3FileKey should be full s3 path
            var getRequest = new GetObjectRequest()
            {
                BucketName = AmazonConstants.ArchivesBucket,
                Key = s3FileKey
            };
            try
            {
                var response = await s3Client.GetObjectAsync(getRequest);
                logger.LogInformation($"Response: {response}");
                logger.LogInformation($"Retrieved {s3FileKey} from {AmazonConstants.ArchivesBucket}");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Failed to write snapshot files: " + ex.Message);
                Console.WriteLine(ex);
                return false;
            }
        }

        // public async Task<bool> SaveZipToS3(string s3fileKey, string zipPath, string snapshotTimeStamp, string snapshotName)
        // {
        //     // var fileKey = Path.Combine(AmazonConstants.SnapshotsDir, snapshotTimeStamp, snapshotName).Replace("\\", "/");
        //     if (s3fileKey.Contains(@"\")) throw new Exception($"S3 file paths cannot have backslash: {s3fileKey}");
        // }
    }
}