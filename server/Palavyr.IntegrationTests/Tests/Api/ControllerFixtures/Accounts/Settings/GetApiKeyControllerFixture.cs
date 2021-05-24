using System.Threading.Tasks;
using Palavyr.API.Controllers.Accounts.Settings;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.FixtureBase;
using Xunit;

namespace Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.Accounts.Settings
{
    public class GetApiKeyControllerFixture : DefaultInMemoryIntegrationFixture
    {
        private const string Route = GetApiKeyController.Uri;

        public GetApiKeyControllerFixture(InMemoryAutofacWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetApiKeyTest()
        {
            var response = await Client.GetAsync(Route);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetApiKeySuccess()
        {
            var response = await Client.GetStringAsync(Route);
            Assert.Equal(response, IntegrationConstants.ApiKey);
        }
    }

    public class ApiKeyFail : BareInMemoryIntegrationFixture
    {
        private const string Route = GetApiKeyController.Uri;

        public ApiKeyFail(InMemoryAutofacWebApplicationFactory factory) : base(factory)
        {
        }


        [Fact]
        public async Task GetApiKeyFails()
        {
            var response = await Client.GetStringAsync(Route);
            Assert.Equal("No Api Key Found", response);
        }
    }
}