using Palavyr.Core.GlobalConstants;
using Shouldly;
using Xunit;

namespace Palavyr.PureTests.Core.GlobalConstants
{
    [Trait("Constants", "Global")]
    public class ConfigSectionsFixture
    {
        [Fact]
        public void ConfigSectionsAreSet()
        {
            ApplicationConstants.ConfigSections.LoggingSection.ShouldBe("Logging");
            ApplicationConstants.ConfigSections.UserDataSection.ShouldBe("Userdata");

            ApplicationConstants.ConfigSections.ConnectionString.ShouldBe("ConnectionString");
            ApplicationConstants.ConfigSections.StripeKeySection.ShouldBe("Stripe:SecretKey");
            ApplicationConstants.ConfigSections.JwtSecretKey.ShouldBe("JWT:SecretKey");

            ApplicationConstants.ConfigSections.AccessKeySection.ShouldBe("AWS:AccessKey");
            ApplicationConstants.ConfigSections.SecretKeySection.ShouldBe("AWS:SecretKey");
        }
    }
}