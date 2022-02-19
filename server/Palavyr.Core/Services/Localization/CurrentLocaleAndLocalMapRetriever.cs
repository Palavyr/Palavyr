using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.Core.Services.Localization
{
    public interface ICurrentLocaleAndLocalMapRetriever
    {
        Task<CurrentLocaleAndLocalMapRetriever.LocaleResponse> GetLocaleDetails(bool read);
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

        public async Task<LocaleResponse> GetLocaleDetails(bool read)
        {
            var account = await accountRepository.GetAccount();
            var localeMeta = localeDefinitions.Parse(account.Locale);

            var localeResponse = new LocaleResponse
            {
                CurrentLocale = localeMeta,
            };

            if (!read)
            {
                var cultureMap = new CultureInfo(localeMeta.Name).CreateLocaleMap();
                localeResponse.AddLocaleMap(cultureMap);
            }

            return localeResponse;
        }

        public class LocaleResponse
        {
            public LocaleResource CurrentLocale { get; set; }
            public LocaleResource[] LocaleMap { get; set; }

            public void AddLocaleMap(LocaleResource[] localeMap)
            {
                LocaleMap = localeMap;
            }
        }
    }
}