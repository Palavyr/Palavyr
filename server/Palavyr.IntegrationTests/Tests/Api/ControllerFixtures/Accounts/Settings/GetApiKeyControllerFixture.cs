using System.Threading.Tasks;
using IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.Core.Handlers.ControllerHandler;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests.Api.ControllerFixtures.Accounts.Settings
{
    public class GetApiKeyControllerFixture : IntegrationTest
    {
        public GetApiKeyControllerFixture(ITestOutputHelper testOutputHelper, ServerFactory factory)
            : base(testOutputHelper, factory)
        {
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