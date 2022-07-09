using System.Net;
using System.Threading.Tasks;
using Palavyr.Core.Handlers.ControllerHandler;
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
        public WhenGettingAnApiKeyForAnAccountThatDoesNotExist(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task GetApiKeyFails()
        {
            var result = await Client.GetHttp<GetApiKeyRequest>(CancellationToken);
            result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        public override async Task InitializeAsync()
        {
            await this.CreateDefaultAccountAndSessionBuilder().WithApiKey(" ").Build();
            await base.InitializeAsync();
        }
    }
}