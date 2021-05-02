using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoDeleter
    {
        Task DeleteLogo(string accountId);
    }

    public class LogoDeleter : ILogoDeleter
    {
        private readonly IConfiguration configuration;
        private readonly DashContext dashContext;
        private readonly IS3Deleter s3Deleter;

        public LogoDeleter(
            IConfiguration configuration,
            DashContext dashContext,
            IS3Deleter s3Deleter
        )
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.s3Deleter = s3Deleter;
        }

        public async Task DeleteLogo(string accountId)
        {
            var userDataBucket = configuration.GetUserDataSection();

            var fileNameMap = dashContext.FileNameMaps.SingleOrDefault(x => x.AccountId == accountId && x.AreaIdentifier == "logo");
            if (fileNameMap != null)
            {
                var success = await s3Deleter.DeleteObjectFromS3Async(userDataBucket, fileNameMap.S3Key);
                if (!success)
                {
                    throw new AmazonS3Exception("Unable to delete logo file from S3");
                }
            }
        }
    }
}