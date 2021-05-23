using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Services.TemporaryPaths;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3Saver
    {
        Task<bool> SaveObjectToS3(string bucket, string localFilePath, string fileKey);
        Task StreamObjectToS3(string bucket, IFormFile stream, string fileKey);
    }

    public class S3Saver : IS3Saver
    {
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<S3Saver> logger;

        public S3Saver(IAmazonS3 s3Client, ITemporaryPath temporaryPath, ILogger<S3Saver> logger)
        {
            this.s3Client = s3Client;
            this.logger = logger;
        }

        [Obsolete]
        public async Task<bool> SaveObjectToS3(string bucket, string localFilePath, string fileKey)
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
                return false;
            }
        }

        public async Task StreamObjectToS3(string bucket, IFormFile formFile, string fileKey)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            if (memoryStream.Length > 10485760) // https://whatsabyte.com/P1/byteconverter.htm how to get the bytes
            {
                // if greater than 10mb, don't allow
                throw new FileTooLargeException("Cannot upload files greater than 10mb");
            }
            var transferUtility = new TransferUtility(s3Client);
            try
            {
                await transferUtility.UploadAsync(memoryStream, bucket, fileKey);
            }
            catch (AmazonS3Exception)
            {
                logger.LogError("Failed to write file to s3");
                throw;
            }
        }
    }
}