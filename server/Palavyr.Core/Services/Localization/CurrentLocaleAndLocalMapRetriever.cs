using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.Core.Handlers
{
    public interface ICurrentLocaleAndLocalMapRetriever
    {
        Task<CurrentLocaleAndLocalMapRetriever.LocaleResponse> GetLocaleDetails();
    }

    public class CurrentLocaleAndLocalMapRetriever : ICurrentLocaleAndLocalMapRetriever
    {
        private readonly IAccountRepository accountRepository;
        private readonly ILogger<CurrentLocaleAndLocalMapRetriever> logger;
        private readonly LocaleDefinitions localeDefinitions;

        public CurrentLocaleAndLocalMapRetriever(IAccountRepository accountRepository, ILogger<CurrentLocaleAndLocalMapRetriever> logger, LocaleDefinitions localeDefinitions)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
            this.localeDefinitions = localeDefinitions;
        }

        public async Task<LocaleResponse> GetLocaleDetails()
        {
            var account = await accountRepository.GetAccount();
            var localeMeta = localeDefinitions.Parse(account.Locale);
            var culture = new CultureInfo(localeMeta.Name);

            return new LocaleResponse
            {
                CurrentLocale = localeMeta,
                LocaleMap = culture.CreateLocaleMap()
            };
        }

        public class LocaleResponse
        {
            public LocaleResource CurrentLocale { get; set; }
            public LocaleResource[] LocaleMap { get; set; }
        }
    }
}