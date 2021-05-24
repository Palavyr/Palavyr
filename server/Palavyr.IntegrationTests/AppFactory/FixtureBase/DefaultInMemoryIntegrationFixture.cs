using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.DataCreators;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.FixtureBase
{
    public class DefaultInMemoryIntegrationFixture : InMemoryIntegrationFixture, IAsyncLifetime
    {
        public DefaultInMemoryIntegrationFixture(ITestOutputHelper testOutputHelper, InMemoryAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        public async Task InitializeAsync()
        {
            await this
                .CreateDefaultAccountAndSessionBuilder()
                .WithDefaultPassword()
                .WithDefaultAccountId()
                .WithDefaultAccountType()
                .WithDefaultApiKey()
                .WithDefaultEmailAddress()
                .Build();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }

    public class BareInMemoryIntegrationFixture : InMemoryIntegrationFixture
    {
        public BareInMemoryIntegrationFixture(ITestOutputHelper testOutputHelper, InMemoryAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}