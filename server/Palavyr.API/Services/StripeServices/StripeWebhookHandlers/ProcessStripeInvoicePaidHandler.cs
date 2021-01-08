using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using EmailService.ResponseEmail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.API.Services.StripeServices.StripeWebhookHandlers
{
    public interface IProcessStripeInvoicePaidHandler
    {
        Task ProcessInvoicePaid(Invoice invoice);
    }

    public class ProcessStripeInvoicePaidHandler : IProcessStripeInvoicePaidHandler
    {
        private readonly ILogger<ProcessStripeInvoicePaidHandler> logger;
        private readonly AccountsContext accountsContext;
        private readonly IStripeSubscriptionService stripeSubscriptionService;
        private readonly ISesEmail emailClient;

        public ProcessStripeInvoicePaidHandler(
            ILogger<ProcessStripeInvoicePaidHandler> processStripeInvoicePaidHandler,
            AccountsContext accountsContext,
            IStripeSubscriptionService stripeSubscriptionService,
            ISesEmail emailClient
        )
        {
            this.logger = processStripeInvoicePaidHandler;
            this.accountsContext = accountsContext;
            this.stripeSubscriptionService = stripeSubscriptionService;
            this.emailClient = emailClient;
        }

        public async Task ProcessInvoicePaid(Invoice invoice)
        {
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
                var subject = "Thanks for you recent payment - From your friends at Palavyr.com";
                var htmlBody = "<h3>Thank you for your recent purchase.";
                var textbody = "Thank you for your recent purchase";
                //TODO: Design a nice email for this payment confirmation.
                
                var ok = await emailClient.SendEmail("palavyr@gmail.com", account.EmailAddress, subject, htmlBody, textbody);
                if (!ok)
                {
                    throw new Exception($"This email should be verified: {account.EmailAddress}");
                }
            }
        }
    }
}