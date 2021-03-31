using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Data;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Stripe;

namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers
{
    public class ProcessStripeInvoicePaidHandler
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
                var textBody = "Thank you for your recent purchase";
                //TODO: Design a nice email for this payment confirmation.
                
                var ok = await emailClient.SendEmail(EmailConstants.PalavyrMainEmailAddress, account.EmailAddress, subject, htmlBody, textBody);
                if (!ok)
                {
                    throw new Exception($"This email should be verified: {account.EmailAddress}");
                }
            }
        }
    }
}