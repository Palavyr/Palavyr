using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.DataCreators;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
{
    public static class TestAccountSetupExtensionMethods
    {
        public static async Task SetupProAccount(this BaseIntegrationFixture baseIntegrationFixture)
        {
            await baseIntegrationFixture
                .CreateDefaultAccountAndSessionBuilder()
                .WithDefaultPassword()
                .WithDefaultAccountId()
                .WithDefaultAccountType()
                .WithDefaultApiKey()
                .WithDefaultEmailAddress()
                .WithProPlan()
                .Build();
        }

        public static async Task SetupLyteAccount(this BaseIntegrationFixture baseIntegrationFixture)
        {
            await baseIntegrationFixture
                .CreateDefaultAccountAndSessionBuilder()
                .WithDefaultPassword()
                .WithDefaultAccountId()
                .WithDefaultAccountType()
                .WithDefaultApiKey()
                .WithDefaultEmailAddress()
                .WithLytePlan()
                .Build();
        }

        public static async Task SetupFreeAccount(this BaseIntegrationFixture baseIntegrationFixture)
        {
            await baseIntegrationFixture
                .CreateDefaultAccountAndSessionBuilder()
                .WithDefaultPassword()
                .WithDefaultAccountId()
                .WithDefaultAccountType()
                .WithDefaultApiKey()
                .WithDefaultEmailAddress()
                .WithFreePlan()
                .Build();
        }
    }
}