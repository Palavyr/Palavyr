using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.FixtureBase;
using Xunit;

namespace Palavyr.IntegrationTests.Tests
{
    public class HealthCheckIntegrationTest : DefaultInMemoryIntegrationFixture
    {
        public HealthCheckIntegrationTest(InMemoryAutofacWebApplicationFactory factory) : base(factory)
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