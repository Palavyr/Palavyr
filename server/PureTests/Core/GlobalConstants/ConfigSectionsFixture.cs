using Palavyr.Core.GlobalConstants;
using Shouldly;
using Xunit;

namespace Pure.Core.GlobalConstants
{
    [Trait("Constants", "Global")]
    public class ConfigSectionsFixture
    {
        [Fact]
        public void ConfigSectionsAreSet()
        {
            ApplicationConstants.ConfigSections.LoggingSection.ShouldBe("Logging");
            ApplicationConstants.ConfigSections.UserDataSection.ShouldBe("Userdata");

            ApplicationConstants.ConfigSections.ConnectionStringPostgres.ShouldBe("ConnectionString");
            ApplicationConstants.ConfigSections.StripeKeySection.ShouldBe("Stripe:SecretKey");
            ApplicationConstants.ConfigSections.JwtSecretKey.ShouldBe("JWTSecretKey");

            ApplicationConstants.ConfigSections.AccessKeySection.ShouldBe("AWS:AccessKey");
            ApplicationConstants.ConfigSections.SecretKeySection.ShouldBe("AWS:SecretKey");
        }
    }
}