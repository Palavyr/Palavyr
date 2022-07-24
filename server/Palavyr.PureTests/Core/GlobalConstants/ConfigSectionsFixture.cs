using Palavyr.Core.GlobalConstants;
using Shouldly;
using Xunit;

namespace Palavyr.PureTests.Core.GlobalConstants
{
    public class ConfigSectionsFixture
    {
        [Fact]
        public void ConfigSectionsAreSet()
        {
            ApplicationConstants.ConfigSections.LoggingSection.ShouldBe("Logging");
            ApplicationConstants.ConfigSections.UserDataSection.ShouldBe("AWS:UserDataBucket");

            ApplicationConstants.ConfigSections.ConnectionString.ShouldBe("ConnectionString");

            ApplicationConstants.ConfigSections.StripeKeySection.ShouldBe("STRIPE:SecretKey");
            ApplicationConstants.ConfigSections.JwtSecretKey.ShouldBe("JWT:SecretKey");

            ApplicationConstants.ConfigSections.AccessKeySection.ShouldBe("AWS:AccessKey");
            ApplicationConstants.ConfigSections.SecretKeySection.ShouldBe("AWS:SecretKey");
            ApplicationConstants.ConfigSections.RegionSection.ShouldBe("AWS:Region");

            ApplicationConstants.ConfigSections.PdfServerUrl.ShouldBe("PDFSERVER:Url");

            ApplicationConstants.ConfigSections.RandomString.ShouldBe("aT5jX*Y7fJEK");
            ApplicationConstants.ConfigSections.CurrentEnvironment.ShouldBe("Environment");
        }
    }
}