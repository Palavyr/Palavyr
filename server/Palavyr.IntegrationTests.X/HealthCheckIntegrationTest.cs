using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Palavyr.API;
using Xunit;

namespace Palavyr.IntegrationTests.X
{
    public class HealthCheckIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        public HealthCheckIntegrationTest(WebApplicationFactory<Startup> factory)
        {
            this.client = factory.CreateDefaultClient();
        }
        
        [Fact]
        public async Task HealthCheckTest()
        {
            var response = await client.GetAsync("/healthcheck");
            response.EnsureSuccessStatusCode();
        }
    }
}