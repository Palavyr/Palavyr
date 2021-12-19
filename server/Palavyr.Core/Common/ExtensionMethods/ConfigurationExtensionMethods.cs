#nullable enable
using Microsoft.Extensions.Configuration;
using Palavyr.Core.GlobalConstants;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class ConfigurationExtensionMethods
    {
        public static string? GetSmtpUsername(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.SmtpUsername).Value;
        }

        public static string? GetSmtpPassword(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.SmtpPassword).Value;
        }
 
        public static string? GetUserDataBucket(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.UserDataSection).Value;
        }

        public static string? GetPreviewBucket(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.PreviewSection).Value;
        }

        public static string? GetStripeKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.StripeKeySection).Value;
        }

        public static string? GetAccessKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.AccessKeySection).Value;
        }

        public static string? GetRegion(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.RegionSection).Value;
        }
        
        public static string? GetSecretKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.SecretKeySection).Value;
        }

        public static string? GetPdfServerPort(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.PdfServerPort).Value;
        }

        public static string? GetPdfServerHost(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.PdfServerHost).Value;
        }

        public static string? GetJwtKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.JwtSecretKey).Value;
        }

        public static string? GetCurrentEnvironment(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.CurrentEnvironment).Value;
        }
    }
}