namespace Palavyr.Common.Constants
{
    public class ConfigSections
    {
        public const string LoggingSection = "Logging";
        public const string PreviewSection = "Previews";
        public const string ConfigurationDbStringKey = "DashContextPostgres";
        public const string AccountDbStringKey = "AccountsContextPostgres";
        public const string ConvoDbStringKey = "ConvoContextPostgres";
        public const string StripeKeySection = "Stripe:SecretKey";
        public const string JwtSecretKey = "JWTSecretKey";
        
        public const string AccessKeySection = "AWS:AccessKey";
        public const string SecretKeySection = "AWS:SecretKey";
    }
}