using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Services.AmazonServices
{
    public interface ILinkCreator
    {
        Task<string> CreateLink(string fileAssetId);
        Task<string[]> CreateManyLinks(string[] fileAssetIds);
    }

    public class AwsS3LinkCreator : ILinkCreator
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IS3PreSignedUrlCreator preSignedUrlCreator;

        public AwsS3LinkCreator(IConfigurationRepository configurationRepository, IS3PreSignedUrlCreator preSignedUrlCreator)
        {
            this.configurationRepository = configurationRepository;
            this.preSignedUrlCreator = preSignedUrlCreator;
        }

        public async Task<string> CreateLink(string fileAssetId)
        {
            var fileAsset = await configurationRepository.GetFileAsset(fileAssetId);
            var link = preSignedUrlCreator.GenericCreatePreSignedUrl(fileAsset.LocationKey);
            return link;
        }

        public async Task<string[]> CreateManyLinks(string[] fileAssetIds)
        {
            var fileAssets = await configurationRepository.GetManyFileAssets(fileAssetIds);
            var links = fileAssets.Select(asset => preSignedUrlCreator.GenericCreatePreSignedUrl(asset.LocationKey));
            return links.ToArray();
        }
    }

    public class Is3PreSignedUrlCreator : IS3PreSignedUrlCreator
    {
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<IS3PreSignedUrlCreator> logger;
        private readonly IConfiguration configuration;

        private readonly DateTime defaultExpiration = DateTime.Now.AddHours(AmazonConstants.PreSignedUrlExpiration);

        public Is3PreSignedUrlCreator(IAmazonS3 s3Client, ILogger<IS3PreSignedUrlCreator> logger, IConfiguration configuration)
        {
            this.s3Client = s3Client;
            this.logger = logger;
            this.configuration = configuration;
        }

        public string GenericCreatePreSignedUrl(string fileKey, DateTime? expiry = null)
        {
            var expiration = expiry ?? defaultExpiration;
            var bucket = configuration.GetUserDataBucket();
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

    public interface IS3PreSignedUrlCreator
    {
        string GenericCreatePreSignedUrl(string fileKey, DateTime? expiry = null);
    }
}