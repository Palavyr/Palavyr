using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Accounts;
using Session = Stripe.Checkout.Session;

namespace Palavyr.API.Services.StripeServices.StripeWebhookHandlers
{
    public interface IProcessStripeCheckoutSessionCompletedHandler
    {
        Task ProcessCheckoutSessionCompleted(Session session);
    }

    public class ProcessStripeCheckoutSessionCompletedHandler : IProcessStripeCheckoutSessionCompletedHandler
    {
        private AccountsContext accountsContext;
        private ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger;
        private StripeSubscriptionService stripeSubscriptionService;
        private StripeProductService stripeProductService;

        public ProcessStripeCheckoutSessionCompletedHandler(
            AccountsContext accountsContext,
            ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger,
            StripeSubscriptionService stripeSubscriptionService,
            StripeProductService stripeProductService
        )
        {
            this.accountsContext = accountsContext;
            this.logger = logger;
            this.stripeSubscriptionService = stripeSubscriptionService;
            this.stripeProductService = stripeProductService;
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
            var planTypeEnum = ProductRegistry.GetPlanTypeEnum(productId);
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