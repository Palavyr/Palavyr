using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Services.EmailService.ResponseEmailTools;
using Stripe;

namespace Palavyr.Services.StripeServices.StripeWebhookHandlers
{
    public class ProcessStripeInvoiceCreatedHandler
    {
        private readonly ILogger<ProcessStripePlanUpdatedHandler> logger;
        private readonly AccountsContext accountsContext;
        private readonly ISesEmail emailClient;

        public ProcessStripeInvoiceCreatedHandler(
            ILogger<ProcessStripePlanUpdatedHandler> logger,
            AccountsContext accountsContext,
            ISesEmail emailClient
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.emailClient = emailClient;
        }

        public async Task ProcessInvoiceCreation(Invoice invoiceCreated)
        {
            logger.LogDebug(invoiceCreated.Currency);
            Console.WriteLine();
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == invoiceCreated.CustomerId);

            if (invoiceCreated.Livemode)
            {
                if (account == null)
                {
                    throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
                }

                var subject = $"Your Palavyr Subscription Invoice for {invoiceCreated.DueDate}";
                var htmlBody = $"<h4>Your Next Palavyr Subscription invoice</h4>Cost:{invoiceCreated.AmountDue} - Due Date: {invoiceCreated.DueDate}";
                var textBody = "Your Next Palavyr Subscription invoice. Cost:{invoiceCreated.AmountDue} - Due Date: {invoiceCreated.DueDate}";
                await emailClient.SendEmail("palavyr@gmail.com", account.EmailAddress, subject, htmlBody, textBody);
            }
        }
    }
}