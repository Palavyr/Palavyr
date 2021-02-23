using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Domain.Accounts.Schemas;
using Subscription = Stripe.Subscription;

namespace Palavyr.Services.StripeServices.StripeWebhookHandlers
{
    public class ProcessStripeSubscriptionDeletedHandler
    {
        private readonly AccountsContext accountsContext;
        private readonly ILogger<ProcessStripeSubscriptionDeletedHandler> logger;

        public ProcessStripeSubscriptionDeletedHandler(
            AccountsContext accountsContext,
            ILogger<ProcessStripeSubscriptionDeletedHandler> logger
        )
        {
            this.accountsContext = accountsContext;
            this.logger = logger;
        }

        public async Task ProcessSubscriptionDeleted(Subscription subscription)
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == subscription.CustomerId);
            if (account == null)
            {
                logger.LogDebug("Error retrieving account by customer ID");
                throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
            }

            account.CurrentPeriodEnd = subscription.CurrentPeriodEnd;
            account.PlanType = Account.PlanTypeEnum.Free;

            await accountsContext.SaveChangesAsync();
        }
    }
}