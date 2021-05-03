﻿using System;
using System.Collections.Generic;
using Amazon.S3;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Services.AmazonServices
{
    public interface ILinkCreator
    {
        string GenericCreatePreSignedUrl(string fileKey, string bucket);
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