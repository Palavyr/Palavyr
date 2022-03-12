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

namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.InvoiceCreated
{
    public class ProcessStripeInvoiceCreatedHandler : INotificationHandler<StripeInvoiceCreatedEvent>
    {
        private readonly ILogger<ProcessStripeInvoiceCreatedHandler> logger;
        private readonly ISesEmail emailClient;
        private readonly IEntityStore<Account> accountStore;

        public ProcessStripeInvoiceCreatedHandler(
            ILogger<ProcessStripeInvoiceCreatedHandler> logger,
            ISesEmail emailClient,
            IEntityStore<Account> accountStore)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.accountStore = accountStore;
        }

        public async Task Handle(StripeInvoiceCreatedEvent notifcation, CancellationToken cancellationToken)
        {
            var invoiceCreated = notifcation.invoice;
            logger.LogDebug(invoiceCreated.Currency);
            Console.WriteLine();

            var account = await accountStore.Get(invoiceCreated.CustomerId, s => s.StripeCustomerId);

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