using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.GlobalConstants;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class ConfigurationExtensionMethods
    {
        public static string CorrectConnectionString(this IConfiguration configuration)
        {
            return configuration
                .GetSectionOrThrow(ApplicationConstants.ConfigSections.ConnectionString)
                .Replace("<SplitMe>", ";");
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

        public static string GetAwsS3ServiceUrl(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.AwsS3ServiceUrl);
        }

        public static string GetAwsSESServiceUrl(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.AwsSesServiceUrl);
        }

        public static string GetPdfUrl(this IConfiguration configuration)
        {
            return configuration.GetSectionOrThrow(ApplicationConstants.ConfigSections.PdfServerUrl);
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
            if (sectionVal is null)
            {
                var sb = new StringBuilder();
                foreach (var (key, value) in configuration.AsEnumerable().ToList())
                {
                    sb.AppendLine($"{key} - {value}");
                }

                sb.AppendLine("__________________________");

                sb.AppendLine($"Failed to load the configuration section: {sectionId}.");
                throw new Exception(sb.ToString());
            }

            return sectionVal;
        }
    }
}