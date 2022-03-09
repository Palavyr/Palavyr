using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Repositories.StoreExtensionMethods
{
    public static class StripeStoreConfigurationMethods
    {
        public static async Task AddStripeEvent(this IConfigurationEntityStore<StripeWebhookRecord> stripeWebhookStore, string id, string signature)
        {
            var newRecord = StripeWebhookRecord.CreateNewRecord(id, signature);
            await stripeWebhookStore.Create(newRecord);
        }
    }
}