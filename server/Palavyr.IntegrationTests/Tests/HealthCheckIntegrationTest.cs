using System;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Palavyr.API;
using Palavyr.IntegrationTests.AppFactory;
using Xunit;

namespace Palavyr.IntegrationTests.Tests
{
    public class HealthCheckIntegrationTest : IClassFixture<IntegrationTestFixtureFactory>, IDisposable
    {
        private HttpClient AuthenticatedClient { get; set; }        

        public HealthCheckIntegrationTest(IntegrationTestFixtureFactory factory)
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