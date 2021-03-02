using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Common.UIDUtils;
using Palavyr.Data;
using Palavyr.Services.EmailService.ResponseEmailTools;
using Stripe;

namespace Palavyr.Services.StripeServices.StripeWebhookHandlers
{
    public class ProcessStripeSubscriptionCreatedHandler
    {
        private readonly AccountsContext accountsContext;
        private readonly ILogger<ProcessStripeSubscriptionCreatedHandler> logger;
        private readonly ISesEmail client;

        public ProcessStripeSubscriptionCreatedHandler(
            AccountsContext accountsContext,
            ILogger<ProcessStripeSubscriptionCreatedHandler> logger,
            ISesEmail client
        )
        {
            this.accountsContext = accountsContext;
            this.logger = logger;
            this.client = client;
        }

        public async Task ProcessSubscriptionCreated(Subscription subscription)
        {
            var account = await subscription.GetAccount(accountsContext, logger);
            var customerEmail = account.EmailAddress;
            var htmlBody = "<p>Thanks so much for subscribing to Palavyr!</p>";
            var textBody = "Thanks so much for subscribing to Palavyr!";
            
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
}