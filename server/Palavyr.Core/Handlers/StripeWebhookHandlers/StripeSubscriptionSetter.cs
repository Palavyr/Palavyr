using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Stores;
using Session = Stripe.Checkout.Session;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public interface IStripeSubscriptionSetter
    {
        Task SetSubscription(Session session, CancellationToken cancellationToken);
    }

    public class StripeSubscriptionSetter : IStripeSubscriptionSetter
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly ILogger<StripeSubscriptionSetter> logger;
        private readonly IStripeSubscriptionService stripeSubscriptionService;

        public StripeSubscriptionSetter(
            IEntityStore<Account> accountStore,
            ILogger<StripeSubscriptionSetter> logger,
            IStripeSubscriptionService stripeSubscriptionService)
        {
            this.accountStore = accountStore;
            this.logger = logger;
            this.stripeSubscriptionService = stripeSubscriptionService;
        }

        public async Task SetSubscription(Session session, CancellationToken cancellationToken)
        {
            var account = await accountStore
                .DangerousRawQuery()
                .SingleOrDefaultAsync(x => x.StripeCustomerId == session.CustomerId);

            if (account == null)
            {
                throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
            }

            var subscription = await stripeSubscriptionService.GetSubscription(session);

            var planTypeEnum = await stripeSubscriptionService.GetPlanTypeEnum(subscription);
            var bufferedPeriodEnd = await stripeSubscriptionService.GetBufferedEndTime(subscription);

            account.PlanType = planTypeEnum;
            account.HasUpgraded = true;
            account.CurrentPeriodEnd = bufferedPeriodEnd;
        }
    }
}