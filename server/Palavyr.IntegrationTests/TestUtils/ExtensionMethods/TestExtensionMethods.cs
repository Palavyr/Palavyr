using Microsoft.Extensions.Configuration;

namespace Palavyr.IntegrationTests.TestUtils.ExtensionMethods
{
    public static class TestExtensionMethods
    {
        public static string GetTestDataSection(this IConfiguration configuration)
        {
            return configuration.GetSection(TestConstants.ConfigSections.TestDataSection).Value;

        }
    }
}