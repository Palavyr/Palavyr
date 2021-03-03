using System;
using System.Net.Http;
using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory;
using Xunit;

namespace Palavyr.IntegrationTests.Tests
{
    public class HealthCheckIntegrationTest : IClassFixture<InMemoryWebApplicationFactory>, IDisposable
    {
        private HttpClient AuthenticatedClient { get; set; }        

        public HealthCheckIntegrationTest(InMemoryWebApplicationFactory factory)
        {
            AuthenticatedClient = factory.ConfigureUnauthenticatedClientWithInMemContext();
        }
        
        [Fact]
        public async Task HealthCheckTest()
        {
            var response = await AuthenticatedClient.GetAsync("/healthcheck");
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            Console.WriteLine("WOW");
        }
    }
}