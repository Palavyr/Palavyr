using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Services.StripeServices.Products;
using Palavyr.Core.Stores;
using Stripe;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripeSubscriptionUpdatedHandler : INotificationHandler<SubscriptionUpdatedEvent>
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly IStripeWebhookAccountGetter stripeWebhookAccountGetter;
        private readonly IStripeSubscriptionService stripeSubscriptionService;
        private readonly IProductRegistry productRegistry;
        private readonly ILogger<ProcessStripeSubscriptionUpdatedHandler> logger;

        public ProcessStripeSubscriptionUpdatedHandler(
            IEntityStore<Account> accountStore,
            IStripeWebhookAccountGetter stripeWebhookAccountGetter,
            IStripeSubscriptionService stripeSubscriptionService,
            IProductRegistry productRegistry,
            ILogger<ProcessStripeSubscriptionUpdatedHandler> logger
        )
        {
            this.accountStore = accountStore;
            this.stripeWebhookAccountGetter = stripeWebhookAccountGetter;
            this.stripeSubscriptionService = stripeSubscriptionService;
            this.productRegistry = productRegistry;
            this.logger = logger;
        }

        public async Task Handle(SubscriptionUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var subscription = @event.Subscription;

            await stripeWebhookAccountGetter.GetAccount(subscription.CustomerId);
            var account = await accountStore.Get(subscription.CustomerId, s => s.StripeCustomerId);

            if (subscription.CancelAtPeriodEnd)
            {
                account.CurrentPeriodEnd = subscription.CurrentPeriodEnd;
                // if we are canceling at period end, then we've cancelled the subscription
                account.PlanType = Models.Accounts.Schemas.Account.PlanTypeEnum.Free;
            }
            else
            {
                var bufferedPeriodEnd = await stripeSubscriptionService.GetBufferedEndTime(subscription);
                account.CurrentPeriodEnd = bufferedPeriodEnd;

                // check the updated subscription type and apply
                var planTypeEnum = await stripeSubscriptionService.GetPlanTypeEnum(subscription);

                account.PlanType = planTypeEnum;
            }
        }
    }

    public class SubscriptionUpdatedEvent : INotification
    {
        public Subscription Subscription { get; }

        public SubscriptionUpdatedEvent(Subscription subscription)
        {
            this.Subscription = subscription;
        }
    }
}