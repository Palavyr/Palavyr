using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Stripe;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripePriceUpdatedHandler : INotificationHandler<PriceUpdatedEvent>
    {
        private readonly ILogger<ProcessStripePriceUpdatedHandler> logger;
        private readonly AccountsContext accountsContext;
        private readonly ISesEmail emailClient;

        public ProcessStripePriceUpdatedHandler(
            ILogger<ProcessStripePriceUpdatedHandler> logger,
            AccountsContext accountsContext,
            ISesEmail emailClient
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.emailClient = emailClient;
        }

        public async Task Handle(PriceUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var priceUpdate = notification.price;
            var accounts = await accountsContext
                .Accounts
                .Where(row => row.PlanType == Account.PlanTypeEnum.Premium || row.PlanType == Account.PlanTypeEnum.Pro)
                .ToListAsync();
            
            if (priceUpdate.Livemode)
            {
                foreach (var account in accounts)
                {
                    // priceUpdate.Product
                    var subject = "Notification of Palavyr Subscription Price Change";
                    var htmlBody = "Thanks for subscribing to Palavyr. We've recently evaluated our operating costs and we need to make a slight adjustment to our pricing."
                                   + $"The new price for {priceUpdate.Product} will be ${priceUpdate.UnitAmount}."
                                   + "We hope you will continue to use Palavyr despite these changes."
                                   + $"Thanks very much, the Palavyr Team";
                    var textBody = "Apologies, your recent payment failed. Please visit the billing tab in the dashboard to update your payment information.";
                    await emailClient.SendEmail(EmailConstants.PalavyrMainEmailAddress, account.EmailAddress, subject, htmlBody, textBody);
                }
            }
        }
    }

    public class PriceUpdatedEvent : INotification
    {
        public readonly Price price;

        public PriceUpdatedEvent(Price price)
        {
            this.price = price;
        }
    }
}