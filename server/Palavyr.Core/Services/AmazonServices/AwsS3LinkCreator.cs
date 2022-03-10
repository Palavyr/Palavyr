using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.AmazonServices
{
    public interface ILinkCreator
    {
        Task<string> CreateLink(string fileAssetId);
        Task<string[]> CreateManyLinks(string[] fileAssetIds);
        Task<string[]> CreateManyLinks(IEnumerable<string> fileAssetIds);
    }

    public class AwsS3LinkCreator : ILinkCreator
    {
        private readonly ILinkCreator linkCreator;
        private readonly IEntityStore<FileAsset> fileAssetStore;


        public AwsS3LinkCreator(ILinkCreator linkCreator, IEntityStore<FileAsset> fileAssetStore)
        {
            this.linkCreator = linkCreator;
            this.fileAssetStore = fileAssetStore;
        }

        public async Task<string> CreateLink(string fileAssetId)
        {
            var link = await linkCreator.CreateLink(fileAssetId);
            return link;
        }

        public async Task<string[]> CreateManyLinks(string[] fileAssetIds)
        {
            var tasks = fileAssetIds.Select(assetId => linkCreator.CreateLink(assetId));
            var links = await Task.WhenAll(tasks);
            return links;
        }

        public async Task<string[]> CreateManyLinks(IEnumerable<string> fileAssetIds)
        {
            return await CreateManyLinks(fileAssetIds.ToArray());
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