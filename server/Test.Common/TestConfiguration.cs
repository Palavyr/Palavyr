using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.GlobalConstants;

namespace Test.Common
{
    public static class TestConfiguration
    {
        public static IConfiguration GetTestConfiguration(Assembly assembly)
        {
            var myConfiguration = new Dictionary<string, string>();
            myConfiguration.Add(ApplicationConstants.ConfigSections.StripeKeySection, "stripe_secret_key_placeholder");
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "Palavyr__")
                .Build();

            return configuration;
        }
    }
}