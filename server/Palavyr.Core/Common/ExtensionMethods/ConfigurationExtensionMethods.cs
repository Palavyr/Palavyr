
using System;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.GlobalConstants;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class ConfigurationExtensionMethods
    {
        public static string GetSmtpUsername(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.SmtpUsername);
        }

        public static string GetSmtpPassword(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.SmtpPassword);
        }

        public static string GetUserDataBucket(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.UserDataSection);
        }
        public static string GetStripeKey(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.StripeKeySection);
        }

        public static string GetAccessKey(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.AccessKeySection);
        }

        public static string GetRegion(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.RegionSection);
        }

        public static string GetSecretKey(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.SecretKeySection);
        }

        public static string GetPdfServerPort(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.PdfServerPort);
        }

        public static string GetPdfServerHost(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.PdfServerHost);
        }

        public static string GetJwtKey(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.JwtSecretKey);
        }

        public static string GetCurrentEnvironment(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.CurrentEnvironment);
        }

        private static string GetSectionOrThrow(this IConfiguration configuration, string sectionId)
        {
            var sectionVal = configuration.GetSection(sectionId).Value;
            if (sectionVal is null) throw new Exception("Failed to load the configuration section.");
            return sectionVal;
        }
    }
}