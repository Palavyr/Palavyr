using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests
{
    public class HealthCheckIntegrationTest : DefaultInMemoryIntegrationFixture
    {
        public HealthCheckIntegrationTest(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task HealthCheckTest()
        {
            var response = await Client.GetAsync("/healthcheck");
            response.EnsureSuccessStatusCode();
        }
    }
}