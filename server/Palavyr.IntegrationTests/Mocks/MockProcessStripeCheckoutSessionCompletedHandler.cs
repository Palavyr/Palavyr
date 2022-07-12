using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Stores;
using Session = Stripe.Checkout.Session;

namespace Palavyr.IntegrationTests.Mocks
{
    public class MockStripeSubscriptionSetter : IStripeSubscriptionSetter
    {
        private readonly IEntityStore<Account> accountStore;

        public MockStripeSubscriptionSetter(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task SetSubscription(Session session, CancellationToken cancellationToken)
        {
            var account = await accountStore
                .DangerousRawQuery()
                .SingleOrDefaultAsync(x => x.StripeCustomerId == session.CustomerId);

            account.PlanType = Account.PlanTypeEnum.Pro;
            account.HasUpgraded = true;
            account.CurrentPeriodEnd = DateTime.Now.AddYears(100);
        }
    }
}