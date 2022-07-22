using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Stores;
using Stripe;
using Account = Palavyr.Core.Data.Entities.Account;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers.SubscriptionCreated
{
    public class ProcessStripeSubscriptionCreatedHandler : INotificationHandler<SubscriptionCreatedEvent>
    {
        private readonly IStripeWebhookAccountGetter stripeWebhookAccountGetter;
        private readonly IEntityStore<Account> accountStore;
        private readonly ILogger<ProcessStripeSubscriptionCreatedHandler> logger;
        private readonly ISesEmail client;

        public ProcessStripeSubscriptionCreatedHandler(
            IStripeWebhookAccountGetter stripeWebhookAccountGetter,
            IEntityStore<Account> accountStore,
            ILogger<ProcessStripeSubscriptionCreatedHandler> logger,
            ISesEmail client
        )
        {
            this.stripeWebhookAccountGetter = stripeWebhookAccountGetter;
            this.accountStore = accountStore;
            this.logger = logger;
            this.client = client;
        }
        
        // to test this in an automated way, we'll need to craete ana ccount, start a silent process
        // for the duration of the test, listen, and then start a second event triggering the 
        // webhook with an event (via the cli). use the --add option to provide the custerom id
        // so that the api can actually find itin the database. Durr
        
        //cus_M2PtMYOgAdgwhS
        // --add [resource]:[path1].[path2]=[value]
        // Add the param path1.path2 to the `resource. Example: --add payment_intent:customer=customerId
        public async Task Handle(SubscriptionCreatedEvent @event, CancellationToken cancellationToken)
        {
            var subscription = @event.subscription;

            var account = await stripeWebhookAccountGetter.GetAccount(subscription.CustomerId);
            
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