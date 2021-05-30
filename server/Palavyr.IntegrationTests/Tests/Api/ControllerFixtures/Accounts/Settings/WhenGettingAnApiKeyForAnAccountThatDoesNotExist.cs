using System.Threading.Tasks;
using Palavyr.API.Controllers.Accounts.Settings;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.Accounts.Settings
{
    public class WhenGettingAnApiKeyForAnAccountThatDoesNotExist : BareInMemoryIntegrationFixture
    {
        private const string Route = GetApiKeyController.Uri;

        public WhenGettingAnApiKeyForAnAccountThatDoesNotExist(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
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