#nullable enable
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoRetriever
    {
        Task<string?> GetLogo();
    }

    public class LogoRetriever : ILogoRetriever
    {
        private readonly IConfiguration configuration;
        private readonly IAccountRepository accountRepository;
        private readonly ILinkCreator linkCreator;

        public LogoRetriever(
            IConfiguration configuration,
            IAccountRepository accountRepository,
            ILinkCreator linkCreator
        )
        {
            this.configuration = configuration;
            this.accountRepository = accountRepository;
            this.linkCreator = linkCreator;
        }

        public async Task<string?> GetLogo()
        {
            var account = await accountRepository.GetAccount();
            var s3Key = account.AccountLogoUri;
            var userDataBucket = configuration.GetUserDataBucket();

            if (string.IsNullOrWhiteSpace(s3Key))
            {
                return null;
            }
            else
            {
                var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3Key, userDataBucket);
                return preSignedUrl;
            }
        }
    }
}