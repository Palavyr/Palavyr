using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Accounts;
using Subscription = Stripe.Subscription;

namespace Palavyr.API.Services.StripeServices.StripeWebhookHandlers
{
    public static class StripeExtensions
    {
        public static async Task<UserAccount> GetAccount(this Subscription subscription, AccountsContext accountsContext, ILogger logger)
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