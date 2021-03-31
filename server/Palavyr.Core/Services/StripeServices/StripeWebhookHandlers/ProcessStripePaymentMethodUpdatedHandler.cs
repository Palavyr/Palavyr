using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Stripe;

namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers
{
    public class ProcessStripePaymentMethodUpdatedHandler
    {
        private readonly ILogger<ProcessStripeInvoicePaidHandler> logger;
        private readonly AccountsContext accountsContext;
        private readonly ISesEmail emailClient;

        public ProcessStripePaymentMethodUpdatedHandler(
            ILogger<ProcessStripeInvoicePaidHandler> processStripeInvoicePaidHandler,
            AccountsContext accountsContext,
            ISesEmail emailClient
        )
        {
            this.logger = processStripeInvoicePaidHandler;
            this.accountsContext = accountsContext;
            this.emailClient = emailClient;
        }

        public async Task ProcessPaymentMethodUpdate(PaymentMethod paymentMethodUpdate)
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == paymentMethodUpdate.CustomerId);

            if (paymentMethodUpdate.Livemode)
            {
                if (account == null)
                {
                    throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status - ProcessStripePaymentMethodUpdate");
                }
            }

            var subject = "Your Palavyr Payment Method Was Updated On Stripe";
            var htmlBody = "<h4>You've successfully updated your Palavyr Payment Method</h4> ";
            var textBody = "You've successfully updated your Palavyr Payment Method.";
            await emailClient.SendEmail("palavyr@gmail.com", account.EmailAddress, subject, htmlBody, textBody);
        }
    }
}