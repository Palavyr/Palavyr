using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.API.Services.StripeServices
{
    public class StripeSubscriptionService
    {
        private StripeClient stripeClient;
        private ILogger<StripeSubscriptionService> logger;
        private SubscriptionService subscriptionService;

        public StripeSubscriptionService(ILogger<StripeSubscriptionService> logger)
        {
            this.stripeClient = new StripeClient(StripeConfiguration.ApiKey);
            this.subscriptionService = new SubscriptionService(stripeClient);
            this.logger = logger;
        }

        public async Task<Subscription> GetSubscription(Session session)
        {
            Subscription subscription;
            try
            {
                subscription = await subscriptionService.GetAsync(session.SubscriptionId);
            }
            catch (StripeException ex)
            {
                logger.LogDebug($"Could not find Stripe Subscription: {ex.Message}");
                throw new Exception($"Could not find Stripe Subscription: {ex.Message}");
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