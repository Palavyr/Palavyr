using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.DataCreators;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
{
    public static class TestAccountSetupExtensionMethods
    {
        public static async Task<Account> SetupProAccount(this BaseIntegrationFixture baseIntegrationFixture)
        {
            return await baseIntegrationFixture
                .SetStandardAccountProperties()
                .WithProPlan()
                .Build();
        }

        public static async Task<Account> SetupLyteAccount(this BaseIntegrationFixture baseIntegrationFixture)
        {
            return await baseIntegrationFixture
                .SetStandardAccountProperties()
                .WithLytePlan()
                .Build();
        }

        public static async Task<Account> SetupFreeAccount(this BaseIntegrationFixture baseIntegrationFixture)
        {
            return await baseIntegrationFixture
                .SetStandardAccountProperties()
                .WithFreePlan()
                .Build();
        }

        private static DefaultAccountAndSessionBuilder SetStandardAccountProperties(this BaseIntegrationFixture baseIntegrationFixture)
        {
            return baseIntegrationFixture
                .CreateDefaultAccountAndSessionBuilder()
                .WithDefaultPassword()
                .WithAccountId(baseIntegrationFixture.AccountId)
                .WithStripeCustomerId(baseIntegrationFixture.StripeCustomerId)
                .WithDefaultAccountType()
                .WithApiKey(baseIntegrationFixture.ApiKey)
                .WithDefaultEmailAddress()
                .AsActive();
        }
    }
}