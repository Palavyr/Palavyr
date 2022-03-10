using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Stores.StoreExtensionMethods
{
    public static class StripeStoreConfigurationMethods
    {
        public static async Task AddStripeEvent(this IEntityStore<StripeWebhookRecord> stripeWebhookStore, string id, string signature)
        {
            var newRecord = StripeWebhookRecord.CreateNewRecord(id, signature);
            await stripeWebhookStore.Create(newRecord);
        }
    }
}