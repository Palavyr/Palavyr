using System.Net;
using System.Threading.Tasks;
using Palavyr.Client;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.Accounts.Settings
{
    public class WhenGettingAnApiKeyForAnAccountThatDoesNotExist : IntegrationTest
    {
        public WhenGettingAnApiKeyForAnAccountThatDoesNotExist(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task GetApiKeyFails()
        {
            var client = new PalavyrClient(WebHostFactory.ConfigureInMemoryApiKeyClient(A.RandomId()));
            var result = await client.GetHttp<GetApiKeyRequest>(CancellationToken);
            result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}