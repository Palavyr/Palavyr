﻿namespace Palavyr.Core.GlobalConstants
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
            public const string RegionSection = "AWS:Region";

            public const string PdfServerHost = "Pdf.Server.Host";
            public const string PdfServerPort = "Pdf.Server.Port";

            public const string SmtpUsername = "AWS:SmtpUsername";
            public const string SmtpPassword = "AWS:SmtpPassword";
            public const string RandomString = "aT5jX*Y7fJEK";
            public const string CurrentEnvironment = "Palavyr.Server.Environment";
        }
    }
}