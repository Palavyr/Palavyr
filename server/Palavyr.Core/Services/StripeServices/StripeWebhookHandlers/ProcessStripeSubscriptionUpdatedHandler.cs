using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Stripe;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers
{
    public class ProcessStripeSubscriptionUpdatedHandler
    {
        private readonly AccountsContext accountsContext;
        private readonly StripeSubscriptionService stripeSubscriptionService;
        private readonly IProductRegistry productRegistry;
        private readonly ILogger<ProcessStripeSubscriptionUpdatedHandler> logger;

        public ProcessStripeSubscriptionUpdatedHandler(
            AccountsContext accountsContext,
            StripeSubscriptionService stripeSubscriptionService,
            IProductRegistry productRegistry,
            ILogger<ProcessStripeSubscriptionUpdatedHandler> logger
        )
        {
            this.accountsContext = accountsContext;
            this.stripeSubscriptionService = stripeSubscriptionService;
            this.productRegistry = productRegistry;
            this.logger = logger;
        }

        public async Task ProcessSubscriptionUpdated(Subscription subscription)
        {
            var account = await subscription.GetAccount(accountsContext, logger);
            if (subscription.CancelAtPeriodEnd)
            {
                account.CurrentPeriodEnd = subscription.CurrentPeriodEnd;
                // if we are canceling at period end, then we've cancelled the subscription
                account.PlanType = Account.PlanTypeEnum.Free;
            }
            else
            {
                var priceDetails = stripeSubscriptionService.GetPriceDetails(subscription);
                var paymentInterval = stripeSubscriptionService.GetPaymentInterval(priceDetails);
                var paymentIntervalEnum = paymentInterval.GetPaymentIntervalEnum();
                var bufferedPeriodEnd = paymentIntervalEnum.AddEndTimeBuffer(subscription.CurrentPeriodEnd);
                account.CurrentPeriodEnd = bufferedPeriodEnd;
                
                // check the updated subscription type and apply
                var productId = stripeSubscriptionService.GetProductId(priceDetails);
                var planTypeEnum = productRegistry.GetPlanTypeEnum(productId);
                account.PlanType = planTypeEnum;
            }

            await accountsContext.SaveChangesAsync();
        }
    }
}