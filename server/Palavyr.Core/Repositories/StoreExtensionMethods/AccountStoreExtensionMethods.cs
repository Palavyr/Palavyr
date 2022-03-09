using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Repositories.StoreExtensionMethods
{
    public static class AccountStoreExtensionMethods
    {
        public static async Task<Account> GetAccount(this IConfigurationEntityStore<Account> accountStore)
        {
            return await accountStore.Get(accountStore.AccountId, s => s.AccountId);
        }
    }
}