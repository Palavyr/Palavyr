using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoDeleter
    {
        Task DeleteLogo();
    }

    public class LogoDeleter : ILogoDeleter
    {
        private readonly IConfiguration configuration;
        private readonly IAccountRepository accountRepository;

        private readonly IS3FileDeleter is3FileDeleter;

        public LogoDeleter(
            IConfiguration configuration,
            IAccountRepository accountRepository,
            IS3FileDeleter is3FileDeleter
        )
        {
            this.configuration = configuration;
            this.accountRepository = accountRepository;
            this.is3FileDeleter = is3FileDeleter;
        }

        public async Task DeleteLogo()
        {
            var account = await accountRepository.GetAccount();

            var s3Key = account.AccountLogoUri;
            if (!string.IsNullOrWhiteSpace(s3Key))
            {
                var userDataBucket = configuration.GetUserDataBucket();
                var success = await is3FileDeleter.DeleteObjectFromS3Async(userDataBucket, s3Key);
                if (!success)
                {
                    throw new AmazonS3Exception("Unable to delete logo file from S3");
                }
            }
            
            account.AccountLogoUri = "";
            await accountRepository.CommitChangesAsync();
        }
    }
}