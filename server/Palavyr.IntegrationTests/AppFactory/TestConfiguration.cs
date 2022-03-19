﻿using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.GlobalConstants;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class TestConfiguration
    {
        public static IConfiguration GetTestConfiguration()
        {
            var myConfiguration = new Dictionary<string, string>();
            myConfiguration.Add(ApplicationConstants.ConfigSections.StripeKeySection, "stripe_secret_key_placeholder");
            myConfiguration.Add(ApplicationConstants.ConfigSections.JwtSecretKey, "jwt_secret_key_section");
            myConfiguration.Add(ApplicationConstants.ConfigSections.UserDataSection, "test-palavyr-userdata");
            myConfiguration.Add(ApplicationConstants.ConfigSections.LoggingSection, "logging_section");


            myConfiguration.Add(ApplicationConstants.ConfigSections.ConfigurationDbStringKey, "DashContextPostgres");
            myConfiguration.Add(ApplicationConstants.ConfigSections.AccountDbStringKey, "AccountsContextPostgres");
            myConfiguration.Add(ApplicationConstants.ConfigSections.ConvoDbStringKey, "ConvoContextPostgres");

            myConfiguration.Add(ApplicationConstants.ConfigSections.AccessKeySection, "AWS:AccessKey");
            myConfiguration.Add(ApplicationConstants.ConfigSections.SecretKeySection, "AWS:SecretKey");
            myConfiguration.Add(ApplicationConstants.ConfigSections.RegionSection, "AWS:Region");

            myConfiguration.Add(ApplicationConstants.ConfigSections.PdfServerHost, "Pdf.Server.Host");
            myConfiguration.Add(ApplicationConstants.ConfigSections.PdfServerPort, "Pdf.Server.Port");


            var assembly = Assembly.GetExecutingAssembly();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .AddUserSecrets(assembly, false)
                .Build();

            return configuration;
        }
    }
}