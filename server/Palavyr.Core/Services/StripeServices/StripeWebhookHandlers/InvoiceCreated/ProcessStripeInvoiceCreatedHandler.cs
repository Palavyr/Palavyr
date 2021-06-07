using System;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Stripe;

namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.InvoiceCreated
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

                var htmlBody = InvoiceCreatedEmail.GetInvoiceCreatedEmailHtml(invoiceCreated.Currency.Humanize(), invoiceCreated.AmountDue.ToString(), invoiceCreated.DueDate.Humanize());
                var textBody = InvoiceCreatedEmail.GetInvoiceCreatedEmailText(invoiceCreated.Currency.Humanize(), invoiceCreated.AmountDue.ToString(), invoiceCreated.DueDate.Humanize());
                await emailClient.SendEmail(
                    EmailConstants.PalavyrMainEmailAddress, 
                    account.EmailAddress, 
                    EmailConstants.PalavyrInvoiceCreatedSubject(invoiceCreated.DueDate), 
                    htmlBody, 
                    textBody);
            }
        }
    }
}