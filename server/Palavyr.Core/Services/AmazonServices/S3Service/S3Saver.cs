using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3Saver
    {
        Task StreamObjectToS3(string bucket, IFormFile stream, string fileKey);
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