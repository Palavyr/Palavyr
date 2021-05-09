#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoRetriever
    {
        Task<string?> GetLogo(string accountId, CancellationToken cancellationToken);
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

        public async Task<string?> GetLogo(string accountId, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount(accountId, cancellationToken);
            var s3Key = account.AccountLogoUri;
            var userDataBucket = configuration.GetUserDataSection();

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