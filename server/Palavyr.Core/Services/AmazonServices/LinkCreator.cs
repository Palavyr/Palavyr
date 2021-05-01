﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods.PathExtensions;
using Palavyr.Core.Common.FileSystemTools.FormPaths;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Services.AmazonServices
{
    public interface ILinkCreator
    {
        string GenericCreatePreSignedUrl(string fileKey, string bucket);
        Task<string> CreatePreSignedUrlResponseLink(string accountId, string fileId, string bucketName);
        Task<string> CreatePreSignedPreviewUrlLink(string accountId, string safeFileNameWithSuffix, string localFilePath, string previewBucket);
        Task<string> CreateAttachmentLinkAsUri(string accountId, string areaId, string fileId, string previewBucket);
        Task<string> CreateLogoImageLinkAsUri(string accountId, string fileName, string localFilePath, string previewBucket);
    }

    public class LinkCreator : ILinkCreator
    {
        private readonly IAmazonS3 s3Client;
        private readonly IS3Saver s3Saver;
        private readonly ILogger<LinkCreator> logger;

        public LinkCreator(IAmazonS3 s3Client, IS3Saver s3Saver, ILogger<LinkCreator> logger)
        {
            this.s3Client = s3Client;
            this.s3Saver = s3Saver;
            this.logger = logger;
        }

        public string GenericCreatePreSignedUrl(string fileKey, string bucket)
        {
            return CreatePreSignedUrl(fileKey, bucket);
        }

        public async Task<string> CreatePreSignedUrlResponseLink(string accountId, string fileId, string bucketName)
        {
            var localFilePath = FormFilePath.FormResponsePDFFilePath(accountId, fileId);
            var fileKey = Path.Combine(accountId, fileId).ConvertToUnix();
            var success = await s3Saver.SaveObjectToS3(bucketName, localFilePath, fileKey);
            if (!success)
            {
                throw new AmazonS3Exception("Could not save preview to S3");
            }

            var preSignedUrl = CreatePreSignedUrl(fileKey, bucketName);
            return preSignedUrl;
        }

        public async Task<string> CreatePreSignedPreviewUrlLink(string accountId, string safeFileNameWithSuffix, string localFilePath, string previewBucket)
        {
            var fileKey = Path.Combine(accountId, safeFileNameWithSuffix).ConvertToUnix();
            var success = await s3Saver.SaveObjectToS3(previewBucket, localFilePath, fileKey);
            if (!success)
            {
                throw new AmazonS3Exception("Could not save preview to S3");
            }

            var preSignedUrl = CreatePreSignedUrl(fileKey, previewBucket);
            return preSignedUrl;
        }

        public async Task<string> CreateAttachmentLinkAsUri(string accountId, string areaId, string fileId, string previewBucket)
        {
            var localFilePath = FormFilePath.FormAttachmentFilePath(accountId, areaId, fileId);
            var fileKey = Path.Combine(accountId, fileId).ConvertToUnix();
            var success = await s3Saver.SaveObjectToS3(previewBucket, localFilePath, fileKey);
            if (!success)
            {
                throw new AmazonS3Exception("Could not save preview to S3");
            }

            var preSignedUrl = CreatePreSignedUrl(fileKey, previewBucket);
            return preSignedUrl;
        }

        public async Task<string> CreateLogoImageLinkAsUri(string accountId, string fileName, string localFilePath, string previewBucket)
        {
            logger.LogDebug("Saving the Logo Image as URI to amazon");
            var fileKey = Path.Combine(accountId, fileName).ConvertToUnix();

            var success = await s3Saver.SaveObjectToS3(previewBucket, localFilePath, fileKey);
            if (!success)
            {
                throw new AmazonS3Exception("Could not save preview to S3");
            }

            var preSignedUrl = CreatePreSignedUrl(fileName, previewBucket);
            return preSignedUrl;
        }


        private string CreatePreSignedUrl(string fileKey, string bucket)
        {
            var expiration = DateTime.Now.AddHours(AmazonConstants.PreSignedUrlExpiration);
            string preSignedUrl;
            try
            {
                preSignedUrl = s3Client.GeneratePreSignedURL(
                    bucket,
                    fileKey,
                    expiration,
                    new Dictionary<string, object>());
            }
            catch (Exception ex)
            {
                logger.LogDebug($"{ex.Message}");
                logger.LogDebug("Failed to create pre-signed Url with s3.");
                throw new AmazonS3Exception("Could not create pre-signed URL to s3");
            }

            return preSignedUrl;
        }
    }
}