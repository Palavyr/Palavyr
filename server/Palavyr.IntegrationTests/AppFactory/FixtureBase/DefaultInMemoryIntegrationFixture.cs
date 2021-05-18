using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.DataBuilders;
using Xunit;

namespace Palavyr.IntegrationTests.AppFactory.FixtureBase
{
    public class DefaultInMemoryIntegrationFixture : InMemoryIntegrationFixture, IAsyncLifetime
    {
        public DefaultInMemoryIntegrationFixture(InMemoryAutofacWebApplicationFactory factory) : base(factory)
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
        public BareInMemoryIntegrationFixture(InMemoryAutofacWebApplicationFactory factory) : base(factory)
        {
        }
    }
}