using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.FormPaths;

namespace Palavyr.Core.Services.AmazonServices
{
    public static class UriUtils
    {
        /// <summary>
        /// Create a presigned URL that links to a document in S3. This is used
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="accountId"></param>
        /// <param name="safeFileNameWithSuffix"></param>
        /// <param name="fromFilePath"></param>
        /// <param name="s3Client"></param>
        /// <param name="previewBucket"></param>
        /// <returns></returns>
        private static async Task<string> CreatePreSignedUrl(ILogger logger, string accountId,
            string safeFileNameWithSuffix, string fromFilePath, IAmazonS3 s3Client, string previewBucket)
        {
            var fileKey = Path.Combine(accountId, safeFileNameWithSuffix).Replace("\\", "/");
            logger.LogDebug($"Using the following path to write to S3: {fileKey}");
            logger.LogDebug($"Writing to bucket: {previewBucket}");
            var putRequest = new PutObjectRequest()
            {
                BucketName = previewBucket,
                FilePath = fromFilePath,
                Key = fileKey
            };
            try
            {
                var response = await s3Client.PutObjectAsync(putRequest);
                logger.LogDebug("Successfully wrote object to S3!");
            }
            catch (Exception ex)
            {
                logger.LogDebug("Failed to write to S3.");
                logger.LogCritical("##### Exception: " + ex.Message);
                throw new Exception();
            }

            var expiration = DateTime.Now.AddHours(3);
            string preSignedUrl;
            try
            {
                preSignedUrl = s3Client.GeneratePreSignedURL(previewBucket, fileKey, expiration,
                    new Dictionary<string, object>());
            }
            catch (Exception ex)
            {
                logger.LogDebug("Failed to create a presigned url");
                logger.LogCritical($"Error: {ex.Message}");
                throw new Exception();
            }
            logger.LogDebug("PreSigned URL: " + preSignedUrl);
            return preSignedUrl;
        }
        
        public static async Task<string> CreatePreSignedPreviewUrlLink(ILogger logger, string accountId, string safeFileNameWithSuffix, string localFilePath, IAmazonS3 s3Client, string previewBucket)
        {
            var preSignedUrl = await CreatePreSignedUrl(logger, accountId, safeFileNameWithSuffix, localFilePath, s3Client, previewBucket);
            return preSignedUrl;
        }

        public static async Task<string> CreatePreSignedUrlResponseLink(
            ILogger logger,
            string accountId, 
            string fileId,
            IAmazonS3 s3Client,
            string previewBucket)
        {
            var fromFilePath = FormFilePath.FormResponsePDFFilePath(accountId, fileId);
            var preSignedUrl = await CreatePreSignedUrl(logger, accountId, fileId, fromFilePath, s3Client, previewBucket);
            return preSignedUrl;
        }
        
        public static async Task<string> CreateAttachmentLinkAsURI(ILogger logger, string accountId, string areaId, string fileId, IAmazonS3 s3Client, string previewBucket)
        {
            var fromFilePath = FormFilePath.FormAttachmentFilePath(accountId, areaId, fileId);
            var preSignedUrl = await CreatePreSignedUrl(logger, accountId, fileId, fromFilePath, s3Client, previewBucket);
            return preSignedUrl;
        }

        public static async Task<string> CreateLogoImageLinkAsURI(
            ILogger logger, 
            string accountId, 
            string fileName,
            string localFilePath,
            IAmazonS3 s3Client,
            string previewBucket)
        {
            logger.LogDebug("Saving the Logo Image as URI to amazon");
            var preSignedUrl = await CreatePreSignedUrl(logger, accountId, fileName, localFilePath, s3Client, previewBucket);
            return preSignedUrl;
        }
    }
}
