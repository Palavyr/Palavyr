using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using EmailService.ResponseEmail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.API.Services.StripeServices.StripeWebhookHandlers
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
            ILogger<ProcessStripeInvoicePaidHandler> processStripeInvoicePaidHandler,
            AccountsContext accountsContext,
            ISesEmail emailClient
        )
        {
            this.logger = processStripeInvoicePaidHandler;
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
                var subject = "Your recent payment to Palavyr.com failed. :(";
                var htmlBody = "<h4>Sincere appologies. Youre recent payment to "
                               + "Palavyr.com failed. Please visit the billing tab in the "
                               + "dashboard to update your payment information."
                               + $" Your subscription will lapse on {endDate}";
                var textBody = "Apologies, your recent payment failed. Please visit the billing tab in the dashboard to update your payment information.";
                // TODO create a nice email that uses the information below to update the customer
                await emailClient.SendEmail("palavyr@gmail.com", account.EmailAddress, subject, htmlBody, textBody);
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