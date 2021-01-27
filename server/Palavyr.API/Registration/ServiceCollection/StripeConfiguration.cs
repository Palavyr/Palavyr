using Microsoft.Extensions.Configuration;
using Palavyr.Common.Constants;
using Stripe;

namespace Palavyr.API.Registration.ServiceCollection
{
    public static class Configurations
    {
        private static readonly int stripeRetriesCount = 3;

        public static void ConfigureStripe(IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration.GetSection(ConfigSections.StripeKeySection).Value;
            StripeConfiguration.MaxNetworkRetries = stripeRetriesCount;
        }
    }
}