using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Palavyr.Common.FileSystem;
using Palavyr.Common.FileSystem.MagicStrings;
using Microsoft.Extensions.Logging;


namespace Palavyr.API.pathUtils
{
    public static class UriUtils
    {

        /// <summary>
        /// Create a presigned URL that links to a document in S3. This is used
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="accountId"></param>
        /// <param name="safeFileNameWithSuffix"></param>
        /// <param name="fromFilePath"></param>
        /// <param name="s3Client"></param>
        /// <returns></returns>
        private static async Task<string> CreatePresignedUrl(ILogger _logger, string accountId, string safeFileNameWithSuffix, string fromFilePath, IAmazonS3 s3Client)
        {
            var fileKey = Path.Combine(accountId, safeFileNameWithSuffix).Replace("\\", "/");
            var putRequest = new PutObjectRequest()
            {
                BucketName = MagicAWSStrings.AWSPreviewBucket,
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
            var preSignedUrl = s3Client.GeneratePreSignedURL(MagicAWSStrings.AWSPreviewBucket, fileKey, expiration,new Dictionary<string, object>());

            Console.WriteLine("PreSigned URL: " + preSignedUrl);
            
            return preSignedUrl;
        }
        
        public static async Task<string> CreatePreSignedPreviewUrlLink(ILogger _logger, string accountId, string safeFileNameWithSuffix, string localFilePath, IAmazonS3 s3Client)
        {
            var preSignedUrl = await CreatePresignedUrl(_logger, accountId, safeFileNameWithSuffix, localFilePath, s3Client);
            return preSignedUrl;
        }

        public static async Task<string> CreatePresignedUrlResponseLink(
            ILogger _logger,
            string accountId, 
            string fileId,
            IAmazonS3 s3Client)
        {
            var fromFilePath = FormFilePath.FormResponsePDFFilePath(accountId, fileId);
            var preSignedUrl = await CreatePresignedUrl(_logger, accountId, fileId, fromFilePath, s3Client);
            return preSignedUrl;
        }
        
        public static async Task<string> CreateAttachmentLinkAsURI(ILogger _logger, string accountId, string areaId, string fileId, IAmazonS3 s3Client)
        {
            var fromFilePath = FormFilePath.FormAttachmentFilePath(accountId, areaId, fileId);
            var preSignedUrl = await CreatePresignedUrl(_logger, accountId, fileId, fromFilePath, s3Client);
            return preSignedUrl;
        }
    }
}
