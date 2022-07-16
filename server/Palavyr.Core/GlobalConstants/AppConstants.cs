namespace Palavyr.Core.GlobalConstants
{
    public class ApplicationConstants
    {
        public class MagicUrlStrings
        {
            public const string SessionAction = "tubmcgubs";
            public const string LogoutAction = "logout";
            public const string Action = "action";
            public const string AccountId = "accountId";
            public const string SessionId = "sessionId";
            public const string ApiKeyAccess = "apiKeyAccess";
            public const string Authorization = "Authorization";
        }

        public class ConfigSections
        {
            public const string LoggingSection = "Logging";
            public const string UserDataSection = "AWS:UserDataBucket";

            public const string ConnectionString = "ConnectionString";

            public const string StripeKeySection = "STRIPE:SecretKey";
            public const string JwtSecretKey = "JWT:SecretKey";

            public const string AccessKeySection = "AWS:AccessKey";
            public const string SecretKeySection = "AWS:SecretKey";
            public const string RegionSection = "AWS:Region";

            public const string PdfServerUrl = "PDFSERVER:Url";

            public const string RandomString = "aT5jX*Y7fJEK";
            public const string CurrentEnvironment = "Environment";
        }
    }
}