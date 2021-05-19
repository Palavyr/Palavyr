namespace Palavyr.IntegrationTests.TestUtils
{
    public class TestConstants
    {
        public class ConfigSections
        {
            public const string TestDataSection = "TestData";

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
        }
    }
}
