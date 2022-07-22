using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Stores.StoreExtensionMethods
{
    public static class AccountStoreExtensionMethods
    {
        public static async Task<Account> GetAccount(this IEntityStore<Account> accountStore)
        {
            return await accountStore.Get(accountStore.AccountId, s => s.AccountId);
        }

        public static async Task<CultureInfo> GetCulture(this IEntityStore<Account> accountStore)
        {
            var account = await accountStore.GetAccount();
            var culture = new CultureInfo(account.Locale);
            return culture;
        }
    }
}