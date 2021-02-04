using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.Extensions.Logging;
using Subscription = Stripe.Subscription;

namespace Palavyr.API.Services.StripeServices.StripeWebhookHandlers
{
    public class ProcessStripeSubscriptionUpdatedHandler
    {
        private readonly AccountsContext accountsContext;
        private readonly StripeSubscriptionService stripeSubscriptionService;
        private readonly ILogger<ProcessStripeSubscriptionUpdatedHandler> logger;

        public ProcessStripeSubscriptionUpdatedHandler(
            AccountsContext accountsContext,
            StripeSubscriptionService stripeSubscriptionService,
            ILogger<ProcessStripeSubscriptionUpdatedHandler> logger
        )
        {
            this.accountsContext = accountsContext;
            this.stripeSubscriptionService = stripeSubscriptionService;
            this.logger = logger;
        }

        public async Task ProcessSubscriptionUpdated(Subscription subscription)
        {
            var account = await subscription.GetAccount(accountsContext, logger);
            if (subscription.CancelAtPeriodEnd)
            {
                account.CurrentPeriodEnd = subscription.CurrentPeriodEnd;
            }
            else
            {
                var priceDetails = stripeSubscriptionService.GetPriceDetails(subscription);
                var paymentInterval = stripeSubscriptionService.GetPaymentInterval(priceDetails);
                var paymentIntervalEnum = paymentInterval.GetPaymentIntervalEnum();
                var bufferedPeriodEnd = paymentIntervalEnum.AddEndTimeBuffer(subscription.CurrentPeriodEnd);
                account.CurrentPeriodEnd = bufferedPeriodEnd;
            }

            await accountsContext.SaveChangesAsync();
        }
    }
}