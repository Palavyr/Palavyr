using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Configuration;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.CloudKeyResolvers;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.FileAssetServices
{
    public interface ICloudFileSaver
    {
        Task<FileAsset> SaveFile(FileName fileName, IFormFile fileData);
    }

    public class AwsCloudFileSaver : ICloudFileSaver
    {
        private readonly IS3FileUploader is3FileUploader;
        private readonly IFileAssetKeyResolver keyResolver;

        private readonly ConfigurationContainer configuration;
        private readonly IAccountIdTransport accountIdTransport;

        public AwsCloudFileSaver(
            IS3FileUploader is3FileUploader,
            IFileAssetKeyResolver keyResolver,
            ConfigurationContainer configuration,
            IAccountIdTransport accountIdTransport
        )
        {
            this.is3FileUploader = is3FileUploader;
            this.keyResolver = keyResolver;
            this.configuration = configuration;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<FileAsset> SaveFile(FileName fileName, IFormFile fileData)
        {
            var userDataBucket = configuration.AwsUserDataBucket;
            var awsFileKey = keyResolver.Resolve(fileName);

            await is3FileUploader.StreamObjectToS3(userDataBucket, fileData, awsFileKey);

            var fileAsset = new FileAsset
            {
                AccountId = accountIdTransport.AccountId,
                Extension = fileName.Extension,
                FileId = fileName.FileId ?? string.Empty,
                LocationKey = awsFileKey,
                RiskyNameStem = fileName.FileStem
            };
            return fileAsset;
        }
    }
}