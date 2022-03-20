using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Stores;
using Stripe;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripeSubscriptionDeletedHandler : INotificationHandler<SubscriptionDeletedEvent>
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly ILogger<ProcessStripeSubscriptionDeletedHandler> logger;

        public ProcessStripeSubscriptionDeletedHandler(
            IEntityStore<Account> accountStore,
            ILogger<ProcessStripeSubscriptionDeletedHandler> logger
        )
        {
            this.accountStore = accountStore;
            this.logger = logger;
        }

        public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
        {
            var subscription = notification.Subscription;

            var account = await accountStore.Get(subscription.CustomerId, s => s.StripeCustomerId);
            if (account == null)
            {
                logger.LogDebug("Error retrieving account by customer ID");
                throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
            }

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