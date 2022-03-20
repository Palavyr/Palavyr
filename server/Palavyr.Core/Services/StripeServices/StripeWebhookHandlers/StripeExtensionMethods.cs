using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores;
using Subscription = Stripe.Subscription;

namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers
{
    public static class StripeExtensionMethods
    {
        public static async Task<Account> GetAccount(this Subscription subscription, IEntityStore<Account> accountStore, ILogger logger)
        {
            var account = await accountStore.Get(subscription.CustomerId, s => s.StripeCustomerId);
            if (account == null)
            {
                logger.LogDebug("Error retrieving account by customer ID");
                throw new DomainException("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
            }
            return account;
        }
    }
}