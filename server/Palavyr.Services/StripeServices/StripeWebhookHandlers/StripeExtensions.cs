using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Domain.Accounts.Schemas;
using Subscription = Stripe.Subscription;

namespace Palavyr.Services.StripeServices.StripeWebhookHandlers
{
    public static class StripeExtensions
    {
        public static async Task<Account> GetAccount(this Subscription subscription, AccountsContext accountsContext, ILogger logger)
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == subscription.CustomerId);
            if (account == null)
            {
                logger.LogDebug("Error retrieving account by customer ID");
                throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
            }
            return account;
        }
    }
}