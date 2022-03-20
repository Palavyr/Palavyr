using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Stores.StoreExtensionMethods
{
    public static class LogoStoreExtensionMethods
    {
        public static async Task<Logo> GetOrCreateLogoRecord(this IEntityStore<Logo> store)
        {
            var logoRecord = await store.GetOrNull(store.AccountId, s => s.AccountId);
            if (logoRecord is null)
            {
                var newRecord = new Logo
                {
                    AccountId = store.AccountId,
                    AccountLogoFileId = ""
                };
                return await store.Create(newRecord);
            }

            return logoRecord;
        }
    }
}