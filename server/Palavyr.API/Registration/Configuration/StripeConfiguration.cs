using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Stripe;

namespace Palavyr.API.Registration.Configuration
{
    public static class Configurations
    {
        private static readonly int stripeRetriesCount = 3;

        public static void ConfigureStripe(IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration.GetStripeKey();
            StripeConfiguration.MaxNetworkRetries = stripeRetriesCount;
        }
    }
}