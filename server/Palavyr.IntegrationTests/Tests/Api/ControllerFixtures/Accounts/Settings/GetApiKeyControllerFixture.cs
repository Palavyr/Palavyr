using System.Threading.Tasks;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.Accounts.Settings
{
    public class GetApiKeyControllerFixture : RealDatabaseIntegrationFixture
    {
        public GetApiKeyControllerFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory)
            : base(testOutputHelper, factory)
        {
        }

        public override async Task InitializeAsync()
        {
            SetCancellationToken();
            SetAccountIdTransport();
            await this.SetupProAccount();
            await base.InitializeAsync();
        }

        [Fact]
        public async Task GetApiKeyTest()
        {
            var response = await Client.GetString<GetApiKeyRequest>(CancellationToken);
            response.ShouldNotBeEmpty();

        }

        [Fact]
        public async Task GetApiKeySuccess()
        {
            var response = await Client.GetString<GetApiKeyRequest>(CancellationToken);
            Assert.Equal(response, ApiKey);
        }
    }
}