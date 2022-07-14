using System.Threading.Tasks;
using IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using IntegrationTests.AppFactory.IntegrationTestFixtures;
using MediatR;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
{
    public class HealthCheckIntegrationTestBase : IntegrationTest
    {
        public HealthCheckIntegrationTestBase(ITestOutputHelper testOutputHelper, ServerFactory factory)
            : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task HealthCheckTest()
        {
            var response = await Client.GetHttp<HealthCheckRequest>(CancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public override async Task DisposeAsync()
        {
            await Task.CompletedTask;
            WebHostFactory.Dispose();
        }
    }

    public class HealthCheckRequest : IRequest<object>
    {
        public const string Route = "/healthcheck";
    }
}