using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Services.StripeServices.Products;
using Stripe;
using Stripe.Checkout;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.Core.Services.StripeServices.CoreServiceWrappers
{
    public class StripeSubscriptionRetriever : IStripeSubscriptionRetriever
    {
        private readonly ILogger<IStripeSubscriptionRetriever> logger;
        private readonly SubscriptionService subscriptionService;

        public StripeSubscriptionRetriever(IStripeClient client, ILogger<IStripeSubscriptionRetriever> logger)
        {
            this.logger = logger;
            subscriptionService = new SubscriptionService(client);
        }

        public virtual async Task<Subscription> GetSubscription(Session session)
        {
            Subscription subscription;
            try
            {
                subscription = await subscriptionService.GetAsync(session.SubscriptionId);
            }
            catch (StripeException ex)
            {
                logger.LogDebug($"Could not find Stripe Subscription: {ex.Message}");
                throw new DomainException($"Could not find Stripe Subscription: {ex.Message}");
            }

            return subscription;
        }
    }

    public interface IStripeSubscriptionRetriever
    {
        Task<Subscription> GetSubscription(Session session);
    }


    public class StripeSubscriptionService : IStripeSubscriptionService
    {
        private readonly IStripeSubscriptionRetriever retriever;
        private ILogger<IStripeSubscriptionService> logger;
        private readonly IProductRegistry productRegistry;


        public StripeSubscriptionService(IStripeSubscriptionRetriever retriever, ILogger<IStripeSubscriptionService> logger, IProductRegistry productRegistry)
        {
            this.retriever = retriever;
            this.logger = logger;
            this.productRegistry = productRegistry;
        }

        public async Task<Account.PlanTypeEnum> GetPlanTypeEnum(Subscription subscription)
        {
            var priceDetails = GetPriceDetails(subscription);
            var productId = GetProductId(priceDetails);
            var planTypeEnum = productRegistry.GetPlanTypeEnum(productId);
            return planTypeEnum;
        }

        public async Task<DateTime> GetBufferedEndTime(Subscription subscription)
        {
            var priceDetails = GetPriceDetails(subscription);
            var paymentInterval = GetPaymentInterval(priceDetails);
            var paymentIntervalEnum = paymentInterval.GetPaymentIntervalEnum();
            var bufferedPeriodEnd = paymentIntervalEnum.AddEndTimeBuffer(subscription.CurrentPeriodEnd);
            return bufferedPeriodEnd;
        }

        // public void wow()
        // {
        //     var subscription = await stripeSubscriptionService.GetSubscription(session);
        //     var planTypeEnum = GetPlanTypeEnum(subscription);
        //     // var priceDetails = stripeSubscriptionService.GetPriceDetails(subscription);
        //     // var productId = stripeSubscriptionService.GetProductId(priceDetails);
        //     // var planTypeEnum = productRegistry.GetPlanTypeEnum(productId);
        //
        //     var bufferedPeriodEnd = stripeSubscriptionServiceGetBufferedEndTime(session);
        //     // var priceDetails = stripeSubscriptionService.GetPriceDetails(subscription);
        //     // var paymentInterval = stripeSubscriptionService.GetPaymentInterval(priceDetails);
        //     // var paymentIntervalEnum = paymentInterval.GetPaymentIntervalEnum();
        //     // var bufferedPeriodEnd = paymentIntervalEnum.AddEndTimeBuffer(subscription.CurrentPeriodEnd);
        // }

        // public virtual async Task<Subscription> GetSubscription(Session session)
        // {
        //     Subscription subscription;
        //     try
        //     {
        //         subscription = await subscriptionService.GetAsync(session.SubscriptionId);
        //     }
        //     catch (StripeException ex)
        //     {
        //         logger.LogDebug($"Could not find Stripe Subscription: {ex.Message}");
        //         throw new DomainException($"Could not find Stripe Subscription: {ex.Message}");
        //     }
        //
        //     return subscription;
        // }

        public Price GetPriceDetails(Subscription subscription)
        {
            var item = subscription.Items.Data.FirstOrDefault();
            if (item == null)
            {
                return null;
            }

            return item.Price;
        }

        public string GetProductId(Price priceDetails)
        {
            return priceDetails.ProductId;
        }

        public string GetPaymentInterval(Price priceDetails)
        {
            var paymentInterval = priceDetails.Recurring.Interval;
            return paymentInterval;
        }

        public async Task<Subscription> GetSubscription(Session session)
        {
            return await retriever.GetSubscription(session);
        }
    }
}