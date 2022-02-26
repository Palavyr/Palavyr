using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Stripe;

namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.InvoiceCreated
{
    public class ProcessStripeInvoiceCreatedHandler : INotificationHandler<StripeInvoiceCreatedEvent>
    {
        private readonly ILogger<ProcessStripeInvoiceCreatedHandler> logger;
        private readonly AccountsContext accountsContext;
        private readonly ISesEmail emailClient;

        public ProcessStripeInvoiceCreatedHandler(
            ILogger<ProcessStripeInvoiceCreatedHandler> logger,
            AccountsContext accountsContext,
            ISesEmail emailClient)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.emailClient = emailClient;
        }

        public async Task Handle(StripeInvoiceCreatedEvent notifcation, CancellationToken cancellationToken)
        {
            var invoiceCreated = notifcation.invoice;
            logger.LogDebug(invoiceCreated.Currency);
            Console.WriteLine();
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == invoiceCreated.CustomerId);

            string dueDate;
            if (invoiceCreated.DueDate is null)
            {
                dueDate = "";
            }
            else
            {
                dueDate = ((DateTime)invoiceCreated.DueDate).ToString("D");
            }

            var htmlBody = InvoiceCreatedEmail.GetInvoiceCreatedEmailHtml(invoiceCreated.Currency, invoiceCreated.AmountDue.ToString(), invoiceCreated.CollectionMethod != "charge_automatically" ? dueDate : "");
            var textBody = InvoiceCreatedEmail.GetInvoiceCreatedEmailText(invoiceCreated.Currency, invoiceCreated.AmountDue.ToString(), invoiceCreated.CollectionMethod != "charge_automatically" ? dueDate : "");
            await emailClient.SendEmail(
                EmailConstants.PalavyrMainEmailAddress,
                account.EmailAddress,
                EmailConstants.PalavyrInvoiceCreatedSubject(invoiceCreated.DueDate),
                htmlBody,
                textBody);
        }
    }

    public class StripeInvoiceCreatedEvent : INotification
    {
        public readonly Invoice invoice;

        public StripeInvoiceCreatedEvent(Invoice invoice)
        {
            this.invoice = invoice;
        }
    }
}