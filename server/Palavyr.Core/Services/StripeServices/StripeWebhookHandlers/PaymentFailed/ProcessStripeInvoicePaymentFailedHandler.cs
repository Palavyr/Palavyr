using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.InvoicePaid;
using Stripe;

namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.PaymentFailed
{
    public interface IProcessStripeInvoicePaymentFailedHandler
    {
        Task ProcessInvoicePaymentFailed(Invoice invoice);
    }

    public class ProcessStripeInvoicePaymentFailedHandler : IProcessStripeInvoicePaymentFailedHandler
    {
        private readonly ILogger<ProcessStripeInvoicePaidHandler> logger;
        private readonly AccountsContext accountsContext;
        private readonly ISesEmail emailClient;

        public ProcessStripeInvoicePaymentFailedHandler(
            ILogger<ProcessStripeInvoicePaidHandler> logger,
            AccountsContext accountsContext,
            ISesEmail emailClient
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.emailClient = emailClient;
        }

        public async Task ProcessInvoicePaymentFailed(Invoice invoice)
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == invoice.CustomerId);

            if (invoice.Livemode)
            {
                if (account == null)
                {
                    throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
                }

                // if we don't get payment, we don't update the currentPeriodEnd. We check this at the beginning of each login, so
                // if we don't udpate, then time moves forward, and eventually the login will set IsActive to false. If isActive is false,
                // then we freeze their account because they owe us money. From there, they can pay their bill, and then cancel if they prefer
                // to not use the service.
                var endDate = account.CurrentPeriodEnd;
                var htmlBody = EmailPaymentFailed.GetPaymentFailedEmailHtml(endDate);
                var textBody = EmailPaymentFailed.GetPaymentFailedEmailText(endDate);
                await emailClient.SendEmail(
                    EmailConstants.PalavyrMainEmailAddress,
                    account.EmailAddress,
                    EmailConstants.PalavyrPaymentFailedSubject,
                    htmlBody,
                    textBody);
            }

            // var emailAddress = account.EmailAddress;
            // var amountDue = invoice.AmountDue;
            // var amountPaid = invoice.AmountPaid;
            // var chargeId = invoice.ChargeId;
            // var hasPaid = invoice.Paid; // should be false
            // var periodStart = invoice.PeriodStart; // DateTime - when the period began
            // var periodEnd = invoice.PeriodEnd; // DateTime - when the period ends

            // use this information to report to the customer using the Email Service (and a new email template for this problem) that
            // their latest invoice payment failed for some reason. Also tell them that they can resolve this by visiting their billing tab on the dashboard.
            // You can also tell them that this for given billing period, and that their subscription will be automatically cancelled a few days after the periodEnd (give
            // them a week or something?)

            // TODO Email the user and inform them that the payment failed. There will be a 2 day grace period before we cancel.
            // The cancel date will be the date provided by the stripe invoice paid webhook + 2 days worth (TODO: Add 2 days in the
            // invoice paid controller.
        }
    }
}