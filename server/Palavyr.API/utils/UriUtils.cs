using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;


namespace Palavyr.API.pathUtils
{
    public static class UriUtils
    {

        private static async Task<string> CreatePresignedUrl(ILogger _logger, string accountId, string fileId, string fromFilePath, IAmazonS3 s3Client)
        {
            var fileKey = Path.Combine(accountId, fileId).Replace("\\", "/");
            var putRequest = new PutObjectRequest()
            {
                BucketName = MagicString.AWSPreviewBucket,
                FilePath = fromFilePath,
                Key = fileKey
            };
            
            try
            {
                var response = await s3Client.PutObjectAsync(putRequest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("##### Exception: " + ex.Message);
            }
            
            var expiration = DateTime.Now.AddHours(3);
            var preSignedUrl = s3Client.GeneratePreSignedURL(MagicString.AWSPreviewBucket, fileKey, expiration,new Dictionary<string, object>());

            Console.WriteLine("PreSigned URL: " + preSignedUrl);
            
            return preSignedUrl;
        }
        
        public static async Task<string> CreatePreSignedPreviewUrlLink(ILogger _logger, string accountId, string fileId, IAmazonS3 s3Client)
        {
            var fromFilePath = DiskUtils.CreatePreviewLinkAsDiskPath(accountId, fileId);
            var preSignedUrl = await CreatePresignedUrl(_logger, accountId, fileId, fromFilePath, s3Client);
            return preSignedUrl;
        }

        public static async Task<string> CreatePresignedUrlResponseLink(
            ILogger _logger,
            string accountId, 
            string fileId,
            IAmazonS3 s3Client)
        {
            var fromFilePath = ResponsePDFPaths.GetResponsePDFAsDiskPath(_logger, accountId, fileId);
            _logger.LogInformation("4. Trying to get presigned URL...");
            var preSignedUrl = await CreatePresignedUrl(_logger, accountId, fileId, fromFilePath, s3Client);
            return preSignedUrl;
        }
        
        public static async Task<string> CreateAttachmentLinkAsURI(ILogger _logger, string accountId, string areaId, string fileId, IAmazonS3 s3Client)
        {
            var fromFilePath = AttachmentPaths.FormAttachmentPath(accountId, areaId, fileId);
            var preSignedUrl = await CreatePresignedUrl(_logger, accountId, fileId, fromFilePath, s3Client);
            return preSignedUrl;
        }
    }
}
