using System.Threading.Tasks;
using Palavyr.API.Controllers.Accounts.Settings;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods.ClientExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.Accounts.Settings
{
    public class GetApiKeyControllerFixture : RealDatabaseIntegrationFixture
    {
        private const string Route = GetApiKeyController.Uri;

        public GetApiKeyControllerFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        public override async Task InitializeAsync()
        {
            SetCancellationToken();
            await this.SetupProAccount();
            await base.InitializeAsync();
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
            var response = await Client.GetAsync<string>(Route);
            Assert.Equal(response, ApiKey);
        }
    }
}