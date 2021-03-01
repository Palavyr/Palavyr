using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Palavyr.API;
using Palavyr.Common.UIDUtils;
using Palavyr.Domain.Accounts.Schemas;
using Palavyr.IntegrationTests.AppFactory;
using Xunit;

namespace Palavyr.IntegrationTests.Tests.ControllerFixtures.Accounts.Settings
{
    public class GetApiKeyControllerFixture : IClassFixture<InMemoryWebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> fixtureFactory;
        private const string Route = "account/settings/api-key";
        public GetApiKeyControllerFixture(InMemoryWebApplicationFactory<Startup> fixtureFactory)
        {
            this.fixtureFactory = fixtureFactory;
        }

        [Fact]
        public async Task GetApiKeyTest()
        {
            var client = fixtureFactory.CreateInMemAuthedClient(DbSetupAndTeardown.SeedTestAccount);
            var response = await client.GetAsync(Route);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetApiKeySuccess()
        {
            var client = fixtureFactory.CreateInMemAuthedClient(DbSetupAndTeardown.SeedTestAccount);
            var response = await client.GetStringAsync(Route);
            Assert.Equal(response, IntegrationConstants.ApiKey);
        }

        [Fact]
        public async Task GetApiKeyFails()
        {
            var client = fixtureFactory.CreateInMemAuthedClient(
                db =>
                {
                    db.Accounts.Add(
                        Account.CreateAccount(
                            IntegrationConstants.EmailAddress,
                            IntegrationConstants.Password,
                            IntegrationConstants.AccountId,
                            null,
                            AccountType.Default
                        ));
                    db.SeedSession(IntegrationConstants.AccountId, null);

                    db.SaveChanges();
                });
            var response = await client.GetStringAsync(Route);
            Assert.Equal(response, "No Api Key Found");
        }
    }
}