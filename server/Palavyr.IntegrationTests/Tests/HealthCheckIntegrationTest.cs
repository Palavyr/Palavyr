using System;
using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.FixtureBase;
using Xunit;

namespace Palavyr.IntegrationTests.Tests
{
    public class HealthCheckIntegrationTest : InMemoryIntegrationFixture, IDisposable
    {
        private readonly InMemoryAutofacWebApplicationFactory factory;

        public HealthCheckIntegrationTest()
        {
            factory = new InMemoryAutofacWebApplicationFactory();
        }

        [Fact]
        public async Task HealthCheckTest()
        {
            var client = this.factory.CreateInMemAuthedClient();
            var response = await client.GetAsync("/healthcheck");
            response.EnsureSuccessStatusCode();
        }
    }
}