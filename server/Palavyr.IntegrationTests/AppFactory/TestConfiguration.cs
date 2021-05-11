﻿using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class TestConfiguration
    {
        public static IConfiguration GetTestConfiguration()
        {
            var myConfiguration = new Dictionary<string, string>();
            myConfiguration.Add("Stripe:SecretKey", "TEST_stripeSecretKey");
            myConfiguration.Add("JWTSecretKey", "SomeSecretKeyagasgasghery336356345345");

            var assembly = Assembly.GetExecutingAssembly();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .AddUserSecrets(assembly, true)
                .Build();

            return configuration;
        }
    }
}