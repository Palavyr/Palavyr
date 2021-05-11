using System.Threading.Tasks;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.FixtureBase;
using Xunit;

namespace Palavyr.IntegrationTests.Tests.ControllerFixtures.Accounts.Settings
{
    public class GetApiKeyControllerFixture : InMemoryIntegrationFixture
    {
        private readonly InMemoryAutofacWebApplicationFactory factory;
        private const string Route = "api/account/settings/api-key";
        public GetApiKeyControllerFixture(InMemoryAutofacWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task GetApiKeyTest()
        {
            var client = factory.CreateInMemAuthedClient();
            var response = await client.GetAsync(Route);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetApiKeySuccess()
        {
            var client = factory.CreateInMemAuthedClient();
            var response = await client.GetStringAsync(Route);
            Assert.Equal(response, IntegrationConstants.ApiKey);
        }

        [Fact]
        public async Task GetApiKeyFails()
        {
            var client = factory.CreateInMemAuthedClient(
                acc =>
                {
                    acc.Accounts.Add(
                        Account.CreateAccount(
                            IntegrationConstants.EmailAddress,
                            IntegrationConstants.Password,
                            IntegrationConstants.AccountId,
                            null,
                            AccountType.Default
                        ));
                    acc.SeedSession(IntegrationConstants.AccountId, null);
                    acc.SaveChanges();
                });
            var response = await client.GetStringAsync(Route);
            Assert.Equal("No Api Key Found", response);
        }
    }
}