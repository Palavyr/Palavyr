using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.GlobalConstants;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class ConfigurationExtensionMethods
    {
        public static string GetUserDataSection(this IConfiguration configuration)
        {
            return configuration.GetSection(ConfigSections.UserDataSection).Value;
        }

        public static string GetPreviewBucket(this IConfiguration configuration)
        {
            return configuration.GetSection(ConfigSections.PreviewSection).Value;
        }

        public static string GetStripeKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ConfigSections.StripeKeySection).Value;
        }

        public static string GetAccessKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ConfigSections.AccessKeySection).Value;
        }

        public static string GetSecretKey(this IConfiguration configuration)
        {
            return configuration.GetSection(ConfigSections.SecretKeySection).Value;
        }
    }
}