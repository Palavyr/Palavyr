using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3FileUploader
    {
        Task StreamObjectToS3(string bucket, IFormFile stream, string fileKey);
    }

    public class Is3FileUploader : IS3FileUploader
    {
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<Is3FileUploader> logger;

        public Is3FileUploader(IAmazonS3 s3Client, ILogger<Is3FileUploader> logger)
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