using Microsoft.Extensions.Configuration;
using Palavyr.Core.Configuration;
using Stripe;

namespace Palavyr.API.Registration.Configuration
{
    public static class Configurations
    {
        private static readonly int stripeRetriesCount = 3;

        public static void ConfigureStripe(ConfigurationContainer configuration)
        {
            StripeConfiguration.ApiKey = configuration.StripeSecret;
            StripeConfiguration.MaxNetworkRetries = stripeRetriesCount;
        }
    }
}