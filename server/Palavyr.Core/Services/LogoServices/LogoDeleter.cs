using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoDeleter
    {
        Task DeleteLogo(string accountId, CancellationToken cancellationToken);
    }

    public class LogoDeleter : ILogoDeleter
    {
        private readonly IConfiguration configuration;
        private readonly DashContext dashContext;
        private readonly AccountsContext accountsContext;
        private readonly IS3Deleter s3Deleter;

        public LogoDeleter(
            IConfiguration configuration,
            DashContext dashContext,
            AccountsContext accountsContext,
            IS3Deleter s3Deleter
        )
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
            this.s3Deleter = s3Deleter;
        }

        public async Task DeleteLogo(string accountId, CancellationToken cancellationToken)
        {
            var account = await accountsContext.Accounts.SingleAsync(x => x.AccountId == accountId);

            var s3Key = account.AccountLogoUri;
            if (!string.IsNullOrWhiteSpace(s3Key))
            {
                var userDataBucket = configuration.GetUserDataSection();
                var success = await s3Deleter.DeleteObjectFromS3Async(userDataBucket, s3Key);
                if (!success)
                {
                    throw new AmazonS3Exception("Unable to delete logo file from S3");
                }
            }
            
            account.AccountLogoUri = "";
            await accountsContext.SaveChangesAsync(cancellationToken);
        }
    }
}