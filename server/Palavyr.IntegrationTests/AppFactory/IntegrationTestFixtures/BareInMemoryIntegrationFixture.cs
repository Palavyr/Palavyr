using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures
{
    public class BareInMemoryIntegrationFixture : InMemoryIntegrationFixture
    {
        public BareInMemoryIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}