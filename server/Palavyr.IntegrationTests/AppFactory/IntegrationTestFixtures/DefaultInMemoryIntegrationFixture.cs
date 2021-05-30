using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.DataCreators;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures
{
    public class DefaultInMemoryIntegrationFixture : InMemoryIntegrationFixture, IAsyncLifetime
    {
        public DefaultInMemoryIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        public virtual async Task InitializeAsync()
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

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}