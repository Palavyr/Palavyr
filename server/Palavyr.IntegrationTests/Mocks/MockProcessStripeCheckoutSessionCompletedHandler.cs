using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.IntegrationTests.Mocks
{
    public class MockProcessStripeCheckoutSessionCompletedHandler : INotificationHandler<CheckoutSessionCompletedNotification>
    {
        private readonly IEntityStore<Account> accountStore;

        public MockProcessStripeCheckoutSessionCompletedHandler(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }
        public async Task Handle(CheckoutSessionCompletedNotification notification, CancellationToken cancellationToken)
        {
            var account = await accountStore.Get(notification.session.CustomerId, s => s.StripeCustomerId);
            account.PlanType = Account.PlanTypeEnum.Pro;
            account.HasUpgraded = true;
            account.CurrentPeriodEnd = DateTime.Now.AddYears(100);
        }
    }
}