using Microsoft.Extensions.Configuration;
using Palavyr.Core.GlobalConstants;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class ConfigurationExtensionMethods
    {
        public static string GetUserDataSection(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.UserDataSection).Value;
        }

        public static string GetPreviewBucket(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.PreviewSection).Value;
        }

        public static string GetStripeKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.StripeKeySection).Value;
        }

        public static string GetAccessKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.AccessKeySection).Value;
        }

        public static string GetRegion(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.RegionSection).Value;
        }
        
        public static string GetSecretKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.SecretKeySection).Value;
        }

        public static string GetJwtKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ApplicationConstants.ConfigSections.JwtSecretKey).Value;
        }
    }
}