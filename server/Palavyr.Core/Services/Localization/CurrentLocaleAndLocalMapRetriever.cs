using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.Localization
{
    public interface ICurrentLocaleAndLocaleMapRetriever
    {
        Task<CurrentLocaleAndLocaleMapRetriever.LocaleResponse> GetLocaleDetails(bool read);
    }

    public class CurrentLocaleAndLocaleMapRetriever : ICurrentLocaleAndLocaleMapRetriever
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly ILogger<CurrentLocaleAndLocaleMapRetriever> logger;
        private readonly ILocaleDefinitions localeDefinitions;

        public CurrentLocaleAndLocaleMapRetriever(IEntityStore<Account> accountStore, ILogger<CurrentLocaleAndLocaleMapRetriever> logger, ILocaleDefinitions localeDefinitions)
        {
            this.accountStore = accountStore;
            this.logger = logger;
            this.localeDefinitions = localeDefinitions;
        }

        public async Task<LocaleResponse> GetLocaleDetails(bool read)
        {
            var account = await accountStore.GetAccount();
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