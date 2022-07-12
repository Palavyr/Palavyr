using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Stores;
using Session = Stripe.Checkout.Session;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeSubscriptionSetter
    {
        Task SetSubscription(Session session, CancellationToken cancellationToken);
    }

    public class StripeSubscriptionSetter : IStripeSubscriptionSetter
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly IStripeWebhookAccountGetter stripeWebhookAccountGetter;
        private readonly ILogger<StripeSubscriptionSetter> logger;
        private readonly IStripeSubscriptionService stripeSubscriptionService;

        public StripeSubscriptionSetter(
            IEntityStore<Account> accountStore,
            IStripeWebhookAccountGetter stripeWebhookAccountGetter,
            ILogger<StripeSubscriptionSetter> logger,
            IStripeSubscriptionService stripeSubscriptionService)
        {
            this.accountStore = accountStore;
            this.stripeWebhookAccountGetter = stripeWebhookAccountGetter;
            this.logger = logger;
            this.stripeSubscriptionService = stripeSubscriptionService;
        }

        public async Task SetSubscription(Session session, CancellationToken cancellationToken)
        {
            var account = await stripeWebhookAccountGetter.GetAccount(session.CustomerId);
            var subscription = await stripeSubscriptionService.GetSubscription(session);

            var planTypeEnum = await stripeSubscriptionService.GetPlanTypeEnum(subscription);
            var bufferedPeriodEnd = await stripeSubscriptionService.GetBufferedEndTime(subscription);

            account.PlanType = planTypeEnum;
            account.HasUpgraded = true;
            account.CurrentPeriodEnd = bufferedPeriodEnd;
        }
    }
}