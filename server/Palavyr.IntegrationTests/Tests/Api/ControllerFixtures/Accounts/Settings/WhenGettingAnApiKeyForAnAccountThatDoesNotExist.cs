using System.Net;
using System.Threading.Tasks;
using Palavyr.API.Controllers.Accounts.Settings;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.DataCreators;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.Accounts.Settings
{
    public class WhenGettingAnApiKeyForAnAccountThatDoesNotExist : InMemoryIntegrationFixture
    {
        private const string Route = GetApiKeyController.Uri;

        public WhenGettingAnApiKeyForAnAccountThatDoesNotExist(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task GetApiKeyFails()
        {
            var result = await Client.GetAsync(Route);
            result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        public override async Task InitializeAsync()
        {
            await this.CreateDefaultAccountAndSessionBuilder().WithApiKey(" ").Build();
            await base.InitializeAsync();
        }
    }
}