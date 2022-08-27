using Microsoft.Extensions.Configuration;
using Palavyr.Core.Configuration;
using Stripe;

namespace Palavyr.API.Registration.Configuration
{
    public static class Configurations
    {
        private static readonly int stripeRetriesCount = 3;

        public static void ConfigureStripe(ConfigContainerServer config)
        {
            StripeConfiguration.ApiKey = config.StripeSecret;
            StripeConfiguration.MaxNetworkRetries = stripeRetriesCount;
        }
    }
}