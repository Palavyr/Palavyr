using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Stores;
using Stripe;
using Account = Palavyr.Core.Data.Entities.Account;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripeSubscriptionDeletedHandler : INotificationHandler<SubscriptionDeletedEvent>
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly IStripeWebhookAccountGetter stripeWebhookAccountGetter;
        private readonly ILogger<ProcessStripeSubscriptionDeletedHandler> logger;

        public ProcessStripeSubscriptionDeletedHandler(
            IEntityStore<Account> accountStore,
            IStripeWebhookAccountGetter stripeWebhookAccountGetter,
            ILogger<ProcessStripeSubscriptionDeletedHandler> logger
        )
        {
            this.accountStore = accountStore;
            this.stripeWebhookAccountGetter = stripeWebhookAccountGetter;
            this.logger = logger;
        }

        public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
        {
            var subscription = notification.Subscription;
            var account = await stripeWebhookAccountGetter.GetAccount(subscription.CustomerId);

            account.CurrentPeriodEnd = subscription.CurrentPeriodEnd;
            account.PlanType = Account.PlanTypeEnum.Free;
        }
    }

    public class SubscriptionDeletedEvent : INotification
    {
        public readonly Subscription Subscription;

        public SubscriptionDeletedEvent(Subscription subscription)
        {
            this.Subscription = subscription;
        }
    }
}