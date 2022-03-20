using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Stores;
using Stripe;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;
namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.SubscriptionCreated
{
    public class ProcessStripeSubscriptionCreatedHandler : INotificationHandler<SubscriptionCreatedEvent>
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly ILogger<ProcessStripeSubscriptionCreatedHandler> logger;
        private readonly ISesEmail client;

        public ProcessStripeSubscriptionCreatedHandler(
            IEntityStore<Account> accountStore,
            ILogger<ProcessStripeSubscriptionCreatedHandler> logger,
            ISesEmail client
        )
        {
            this.accountStore = accountStore;
            this.logger = logger;
            this.client = client;
        }

        public async Task Handle(SubscriptionCreatedEvent @event, CancellationToken cancellationToken)
        {
            var subscription = @event.subscription;

            var account = await subscription.GetAccount(accountStore, logger);
            var customerEmail = account.EmailAddress;
            var htmlBody = SubscriptionCreated.GetSubscriptionCreatedHtml();
            var textBody = SubscriptionCreated.GetSubscriptionCreatedText();

            var ok = await client.SendEmail(
                EmailConstants.PalavyrMainEmailAddress,
                customerEmail,
                EmailConstants.PalavyrSubscriptionCreateSubject,
                htmlBody,
                textBody);

            if (!ok)
            {
                throw new Exception($"Failed to send an email to {customerEmail}");
            }
        }
    }

    public class SubscriptionCreatedEvent : INotification
    {
        public readonly Subscription subscription;

        public SubscriptionCreatedEvent(Subscription subscription)
        {
            this.subscription = subscription;
        }
    }
}