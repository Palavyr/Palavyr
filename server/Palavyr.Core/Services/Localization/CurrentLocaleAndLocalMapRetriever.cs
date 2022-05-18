using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.Localization
{
    public interface ICurrentLocaleAndLocaleMapRetriever
    {
        Task<LocaleMetaResource> GetLocaleDetails(bool read);
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

        public async Task<LocaleMetaResource> GetLocaleDetails(bool read)
        {
            var account = await accountStore.GetAccount();
            var localeMeta = localeDefinitions.Parse(account.Locale);

            var localeMetaResource = new LocaleMetaResource
            {
                CurrentLocale = localeMeta,
            };

            if (!read)
            {
                var cultureMap = new CultureInfo(localeMeta.Name).CreateLocaleMap();
                localeMetaResource.AddLocaleMap(cultureMap);
            }

            return localeMetaResource;
        }

    }
}