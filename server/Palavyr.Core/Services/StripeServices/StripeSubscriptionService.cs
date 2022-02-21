using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Services.StripeServices.Products;
using Stripe;
using Stripe.Checkout;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeSubscriptionService
    {
        Task<DateTime> GetBufferedEndTime(Subscription subscription);
        Task<Account.PlanTypeEnum> GetPlanTypeEnum(Subscription subscription);


        Task<Subscription> GetSubscription(Session session);
        Price GetPriceDetails(Subscription subscription);
        string GetProductId(Price priceDetails);
        string GetPaymentInterval(Price priceDetails);
    }

    public class StripeSubscriptionService : IStripeSubscriptionService
    {
        private ILogger<IStripeSubscriptionService> logger;
        private readonly IStripeServiceLocatorProvider stripeServiceLocatorProvider;
        private readonly IProductRegistry productRegistry;


        public StripeSubscriptionService(ILogger<IStripeSubscriptionService> logger, IStripeServiceLocatorProvider stripeServiceLocatorProvider, IProductRegistry productRegistry)
        {
            this.logger = logger;
            this.stripeServiceLocatorProvider = stripeServiceLocatorProvider;
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

        public async Task<Subscription> GetSubscription(Session session)
        {
            Subscription subscription;
            try
            {
                subscription = await stripeServiceLocatorProvider.SubscriptionService.GetAsync(session.SubscriptionId);
            }
            catch (StripeException ex)
            {
                logger.LogDebug($"Could not find Stripe Subscription: {ex.Message}");
                throw new DomainException($"Could not find Stripe Subscription: {ex.Message}");
            }

            return subscription;
        }

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
    }
}