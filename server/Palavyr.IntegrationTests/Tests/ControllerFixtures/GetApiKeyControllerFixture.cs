using System.Threading.Tasks;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Palavyr.API;
using Palavyr.IntegrationTests.AppFactory;
using Xunit;

namespace Palavyr.IntegrationTests.Tests.ControllerFixtures
{
    public class GetApiKeyControllerFixture : IntegrationAppFactoryBase
    {
        private const string Route = "account/settings/api-key";
        public GetApiKeyControllerFixture(ClaimInjectorWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task GetApiKeyTest()
        {
            var client = Factory.ConfigureAuthenticatedClientWithInMemContext(DbSetupAndTeardown.SeedTestAccount);
            var response = await client.GetAsync(Route);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetApiKeySuccess()
        {
            var client = Factory.ConfigureAuthenticatedClientWithInMemContext(DbSetupAndTeardown.SeedTestAccount);
            var response = await client.GetStringAsync(Route);
            Assert.Equal(response, IntegrationConstants.ApiKey);
        }
    }
}