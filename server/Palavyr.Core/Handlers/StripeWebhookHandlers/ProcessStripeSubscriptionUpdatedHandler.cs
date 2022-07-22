using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Stripe;
using Account = Palavyr.Core.Data.Entities.Account;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripeSubscriptionUpdatedHandler : INotificationHandler<SubscriptionUpdatedEvent>
    {
        private readonly IStripeWebhookAccountGetter stripeWebhookAccountGetter;
        private readonly IStripeSubscriptionService stripeSubscriptionService;

        public ProcessStripeSubscriptionUpdatedHandler(
            IStripeWebhookAccountGetter stripeWebhookAccountGetter,
            IStripeSubscriptionService stripeSubscriptionService
        )
        {
            this.stripeWebhookAccountGetter = stripeWebhookAccountGetter;
            this.stripeSubscriptionService = stripeSubscriptionService;
        }

        public async Task Handle(SubscriptionUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var subscription = @event.Subscription;

            var account = await stripeWebhookAccountGetter.GetAccount(subscription.CustomerId);

            if (subscription.CancelAtPeriodEnd)
            {
                account.CurrentPeriodEnd = subscription.CurrentPeriodEnd;
                // if we are canceling at period end, then we've cancelled the subscription
                account.PlanType = Account.PlanTypeEnum.Free;
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