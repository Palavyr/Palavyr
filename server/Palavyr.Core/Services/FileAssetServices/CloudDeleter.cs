using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Configuration;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Services.FileAssetServices
{
    public interface ICloudDeleter
    {
        Task Delete(FileAsset fileAsset);
        Task DeleteMany(List<FileAsset> fileAssets);
    }

    public class CloudDeleter : ICloudDeleter
    {
        private readonly IS3FileDeleter is3FileDeleter;
        private readonly ConfigurationContainer configuration;

        public CloudDeleter(IS3FileDeleter is3FileDeleter, ConfigurationContainer configuration)
        {
            this.is3FileDeleter = is3FileDeleter;
            this.configuration = configuration;
        }

        public string UserDataBucket => configuration.AwsUserDataBucket;

        public async Task Delete(FileAsset fileAsset)
        {
            var success = await is3FileDeleter.DeleteObjectFromS3Async(UserDataBucket, fileAsset.LocationKey);
            if (!success)
            {
                throw new DomainException("Could not delete files from the server");
            }
        }

        public async Task DeleteMany(List<FileAsset> fileAssets)
        {
            var success = await is3FileDeleter.DeleteObjectsFromS3Async(UserDataBucket, fileAssets.Select(x => x.LocationKey));
            if (!success)
            {
                throw new DomainException("Could not delete files from the server");
            }
        }
    }
}