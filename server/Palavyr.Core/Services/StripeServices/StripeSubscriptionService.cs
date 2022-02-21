using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeSubscriptionService
    {
        Task<Subscription> GetSubscription(Session session);
        Price GetPriceDetails(Subscription subscription);
        string GetProductId(Price priceDetails);
        string GetPaymentInterval(Price priceDetails);
    }

    public class StripeSubscriptionService : IStripeSubscriptionService
    {
        private ILogger<IStripeSubscriptionService> logger;
        private readonly IStripeServiceLocatorProvider stripeServiceLocatorProvider;


        public StripeSubscriptionService(ILogger<IStripeSubscriptionService> logger, IStripeServiceLocatorProvider stripeServiceLocatorProvider)
        {
            this.logger = logger;
            this.stripeServiceLocatorProvider = stripeServiceLocatorProvider;
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