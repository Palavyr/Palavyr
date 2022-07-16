using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Stores.StoreExtensionMethods
{
    public static class StripeStoreConfigurationMethods
    {
        public static async Task AddStripeEvent(this IEntityStore<StripeWebhookReceivedRecord> stripeWebhookStore, string id, string signature)
        {
            var newRecord = StripeWebhookReceivedRecord.CreateNewRecord(id, signature);
            await stripeWebhookStore.Create(newRecord);
        }
    }
}