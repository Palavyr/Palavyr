using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests
{
    public class HealthCheckIntegrationTest : InMemoryIntegrationFixture
    {
        public HealthCheckIntegrationTest(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory)
            : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task HealthCheckTest()
        {
            var response = await Client.Client.GetAsync("/healthcheck");
            response.EnsureSuccessStatusCode();
        }

        public override async Task DisposeAsync()
        {
            await Task.CompletedTask;
            WebHostFactory.Dispose();
        }
    }
}