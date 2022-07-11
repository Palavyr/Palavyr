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

namespace Palavyr.Core.Handlers.StripeWebhookHandlers.InvoicePaid
{
    public class ProcessStripeInvoicePaymentSuccessHandler : INotificationHandler<InvoicePaymentSuccessfulEvent>
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly IStripeWebhookAccountGetter stripeWebhookAccountGetter;
        private readonly ILogger<ProcessStripeInvoicePaymentSuccessHandler> logger;
        private readonly ISesEmail emailClient;

        public ProcessStripeInvoicePaymentSuccessHandler(
            IEntityStore<Account> accountStore,
            IStripeWebhookAccountGetter stripeWebhookAccountGetter,
            ILogger<ProcessStripeInvoicePaymentSuccessHandler> logger,
            ISesEmail emailClient
        )
        {
            this.accountStore = accountStore;
            this.stripeWebhookAccountGetter = stripeWebhookAccountGetter;
            this.logger = logger;
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
            // }
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