using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Services.StripeServices.Products;
using Stripe.Checkout;

namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers
{
    public interface IProcessStripeCheckoutSessionCompletedHandler
    {
        Task ProcessCheckoutSessionCompleted(Session session);
    }

    public class ProcessStripeCheckoutSessionCompletedHandler : IProcessStripeCheckoutSessionCompletedHandler
    {
        private AccountsContext accountsContext;
        private readonly IProductRegistry productRegistry;
        private ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger;
        private StripeSubscriptionService stripeSubscriptionService;

        public ProcessStripeCheckoutSessionCompletedHandler(
            AccountsContext accountsContext,
            IProductRegistry productRegistry,
            ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger,
            StripeSubscriptionService stripeSubscriptionService
        )
        {
            this.accountsContext = accountsContext;
            this.productRegistry = productRegistry;
            this.logger = logger;
            this.stripeSubscriptionService = stripeSubscriptionService;
        }

        /// <summary>
        /// Payment is successful and the subscription is created.
        /// You should provision the subscription.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task ProcessCheckoutSessionCompleted(Session session)
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == session.CustomerId);
            if (account == null)
            {
                throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
            }

            var subscription = await stripeSubscriptionService.GetSubscription(session);
            var priceDetails = stripeSubscriptionService.GetPriceDetails(subscription);
            var productId = stripeSubscriptionService.GetProductId(priceDetails);
            var planTypeEnum = productRegistry.GetPlanTypeEnum(productId);
            var paymentInterval = stripeSubscriptionService.GetPaymentInterval(priceDetails);
            var paymentIntervalEnum = paymentInterval.GetPaymentIntervalEnum();
            var bufferedPeriodEnd = paymentIntervalEnum.AddEndTimeBuffer(subscription.CurrentPeriodEnd);

            account.PlanType = planTypeEnum;
            account.HasUpgraded = true;
            account.CurrentPeriodEnd = bufferedPeriodEnd;
            await accountsContext.SaveChangesAsync();
        }
        
    }
}