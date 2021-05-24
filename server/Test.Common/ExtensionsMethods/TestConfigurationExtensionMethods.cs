﻿using Microsoft.Extensions.Configuration;
using Test.Common.Constants;

namespace Test.Common.ExtensionsMethods
{
    public static class TestConfigurationExtensionMethods
    {
        public static string GetUserDataBucket(this IConfiguration configuration)
        {
            return configuration.GetSection(TestConstants.ConfigSections.UserDataSection).Value;
        }

        public static string GetPreviewBucket(this IConfiguration configuration)
        {
            return configuration.GetSection(TestConstants.ConfigSections.PreviewSection).Value;
        }

        public static string GetStripeKey(this IConfiguration configuration)
        {
            return configuration.GetSection(TestConstants.ConfigSections.StripeKeySection).Value;
        }

        public static string GetAccessKey(this IConfiguration configuration)
        {
            return configuration.GetSection(TestConstants.ConfigSections.AccessKeySection).Value;
        }

        public static string GetRegion(this IConfiguration configuration)
        {
            return configuration.GetSection(TestConstants.ConfigSections.RegionSection).Value;
        }
        
        public static string GetSecretKey(this IConfiguration configuration)
        {
            return configuration.GetSection(TestConstants.ConfigSections.SecretKeySection).Value;
        }

        public static string GetPdfServerPort(this IConfiguration configuration)
        {
            return configuration.GetSection(TestConstants.ConfigSections.PdfServerPort).Value;
        }

        public static string GetPdfServerHost(this IConfiguration configuration)
        {
            return configuration.GetSection(TestConstants.ConfigSections.PdfServerHost).Value;
        }

        public static string GetJwtKey(this IConfiguration configuration)
        {
            return configuration.GetSection(TestConstants.ConfigSections.JwtSecretKey).Value;
        }
    }
}