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
                .CreateDefaultAccountAndSessionBuilder()
                .WithDefaultPassword()
                .WithAccountId(baseIntegrationFixture.AccountId)
                .WithDefaultAccountType()
                .WithApiKey(baseIntegrationFixture.ApiKey)
                .WithDefaultEmailAddress()
                .WithProPlan()
                .Build();
        }

        public static async Task<Account> SetupLyteAccount(this BaseIntegrationFixture baseIntegrationFixture)
        {
            return await baseIntegrationFixture
                .CreateDefaultAccountAndSessionBuilder()
                .WithDefaultPassword()
                .WithAccountId(baseIntegrationFixture.AccountId)
                .WithDefaultAccountType()
                .WithApiKey(baseIntegrationFixture.ApiKey)
                .WithDefaultEmailAddress()
                .WithLytePlan()
                .Build();
        }

        public static async Task<Account> SetupFreeAccount(this BaseIntegrationFixture baseIntegrationFixture)
        {
            return await baseIntegrationFixture
                .CreateDefaultAccountAndSessionBuilder()
                .WithDefaultPassword()
                .WithAccountId(baseIntegrationFixture.AccountId)
                .WithDefaultAccountType()
                .WithApiKey(baseIntegrationFixture.ApiKey)
                .WithDefaultEmailAddress()
                .WithFreePlan()
                .Build();
        }
    }
}