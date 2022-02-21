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

namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.InvoicePaid
{
    public class ProcessStripeInvoicePaidHandler : INotificationHandler<InvoicePaidEvent>
    {
        private readonly ILogger<ProcessStripeInvoicePaidHandler> logger;
        private readonly AccountsContext accountsContext;
        private readonly ISesEmail emailClient;

        public ProcessStripeInvoicePaidHandler(
            ILogger<ProcessStripeInvoicePaidHandler> processStripeInvoicePaidHandler,
            AccountsContext accountsContext,
            ISesEmail emailClient
        )
        {
            this.logger = processStripeInvoicePaidHandler;
            this.accountsContext = accountsContext;
            this.emailClient = emailClient;
        }

        public async Task Handle(InvoicePaidEvent notification, CancellationToken cancellationToken)
        {
            var invoice = notification.invoice;
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == invoice.CustomerId);
            if (invoice.Livemode)
            {
                if (account == null)
                {
                    logger.LogDebug("Error retrieving account by customer ID");
                    throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
                }

                account.CurrentPeriodEnd = invoice.Subscription.CurrentPeriodEnd;
                await accountsContext.SaveChangesAsync();

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
    }

    public class InvoicePaidEvent : INotification
    {
        public readonly Invoice invoice;

        public InvoicePaidEvent(Invoice invoice)
        {
            this.invoice = invoice;
        }
    }
}