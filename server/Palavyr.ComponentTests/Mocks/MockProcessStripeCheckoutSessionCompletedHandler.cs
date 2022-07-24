using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Stores;
using Session = Stripe.Checkout.Session;

namespace Palavyr.Component.Mocks
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

            if (account is null) throw new Exception($"The account was null! - this is the problem: StripeCustId: {session.CustomerId}");
            
            
            
            account.PlanType = Account.PlanTypeEnum.Pro;
            account.HasUpgraded = true;
            account.CurrentPeriodEnd = TimeUtils.CreateNewTimeStamp().AddYears(100);
        }
    }
}