using System;
using System.IO;

namespace Palavyr.Core.GlobalConstants
{
    public class ApplicationConstants
    {
        public class MagicUrlStrings
        {
            public const string SessionAction = "tubmcgubs";
            public const string Action = "action";
            public const string AccountId = "accountId";
            public const string SessionId = "sessionId";
            public const string ApiKeyAccess = "apiKeyAccess";
            public const string DevAccess = "secretDevAccess";
            public const string DevAccount = "dashboardDev";
        }

        public class SubscriptionConstants
        {
            public const int DefaultNumAreas = 2;
            public const int PremiumNumAreas = 6;
            public const int ProNumAreas = 99999999;
        }

        public class ConfigSections
        {
            public const string LoggingSection = "Logging";
            public const string PreviewSection = "Previews";
            public const string UserDataSection = "Userdata";

            public const string ConfigurationDbStringKey = "DashContextPostgres";
            public const string AccountDbStringKey = "AccountsContextPostgres";
            public const string ConvoDbStringKey = "ConvoContextPostgres";
            public const string StripeKeySection = "Stripe:SecretKey";
            public const string JwtSecretKey = "JWTSecretKey";

            public const string AccessKeySection = "AWS:AccessKey";
            public const string SecretKeySection = "AWS:SecretKey";
        }
    }
}