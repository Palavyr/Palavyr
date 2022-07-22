using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices;
using Stripe;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers.InvoiceCreated
{
    public class ProcessStripeInvoiceCreatedHandler : INotificationHandler<StripeInvoiceCreatedEvent>
    {
        private readonly IStripeWebhookAccountGetter stripeWebhookAccountGetter;
        private readonly ILogger<ProcessStripeInvoiceCreatedHandler> logger;
        private readonly ISesEmail emailClient;

        public ProcessStripeInvoiceCreatedHandler(
            IStripeWebhookAccountGetter stripeWebhookAccountGetter,
            ILogger<ProcessStripeInvoiceCreatedHandler> logger,
            ISesEmail emailClient)
        {
            this.stripeWebhookAccountGetter = stripeWebhookAccountGetter;
            this.logger = logger;
            this.emailClient = emailClient;
        }

        public async Task Handle(StripeInvoiceCreatedEvent notification, CancellationToken cancellationToken)
        {
            var invoiceCreated = notification.invoice;
            logger.LogDebug("{Currency}", invoiceCreated.Currency);

            var account = await stripeWebhookAccountGetter.GetAccount(invoiceCreated.CustomerId);

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