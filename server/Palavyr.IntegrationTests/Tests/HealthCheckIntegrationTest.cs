using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Palavyr.API;
using Palavyr.IntegrationTests.AppFactory;
using Xunit;

namespace Palavyr.IntegrationTests.Tests
{
    public class HealthCheckIntegrationTest : IntegrationAppFactoryBase
    {
        private HttpClient AuthenticatedClient { get; set; }        

        public HealthCheckIntegrationTest(ClaimInjectorWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticatedClient = factory.ConfigureUnauthenticatedClientWithInMemContext();
        }
        
        [Fact]
        public async Task HealthCheckTest()
        {
            var response = await AuthenticatedClient.GetAsync("/healthcheck");
            response.EnsureSuccessStatusCode();
        }
    }
}