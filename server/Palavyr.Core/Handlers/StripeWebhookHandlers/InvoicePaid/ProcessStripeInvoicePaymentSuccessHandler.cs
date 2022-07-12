using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices;
using Stripe;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers.InvoicePaid
{
    public class ProcessStripeInvoicePaymentSuccessHandler : INotificationHandler<InvoicePaymentSuccessfulEvent>
    {
        private readonly IStripeWebhookAccountGetter stripeWebhookAccountGetter;
        private readonly ISesEmail emailClient;

        public ProcessStripeInvoicePaymentSuccessHandler(
            IStripeWebhookAccountGetter stripeWebhookAccountGetter,
            ISesEmail emailClient
        )
        {
            this.stripeWebhookAccountGetter = stripeWebhookAccountGetter;
            this.emailClient = emailClient;
        }

        public async Task Handle(InvoicePaymentSuccessfulEvent notification, CancellationToken cancellationToken)
        {
            var invoice = notification.Invoice;
            var account = await stripeWebhookAccountGetter.GetAccount(invoice.CustomerId);

            account.CurrentPeriodEnd = invoice.Subscription.CurrentPeriodEnd;

            var htmlBody = PaymentSucceededEmail.GetPaymentSucceededEmailHtml(invoice.Subscription.CurrentPeriodEnd);
            var textBody = PaymentSucceededEmail.GetPaymentSucceededEmailText(invoice.Subscription.CurrentPeriodEnd);

            var ok = await emailClient.SendEmail(
                EmailConstants.PalavyrMainEmailAddress,
                account.EmailAddress,
                EmailConstants.PalavyrPaymentSucceededSubject,
                htmlBody,
                textBody);
            if (!ok)
            {
                throw new Exception($"This email should be verified: {account.EmailAddress}");
            }
        }
    }

    public class InvoicePaymentSuccessfulEvent : INotification
    {
        public readonly Invoice Invoice;

        public InvoicePaymentSuccessfulEvent(Invoice invoice)
        {
            this.Invoice = invoice;
        }
    }
}