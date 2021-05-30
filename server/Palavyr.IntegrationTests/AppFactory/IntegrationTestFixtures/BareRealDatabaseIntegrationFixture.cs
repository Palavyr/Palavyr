using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures
{
    public class BareRealDatabaseIntegrationFixture : RealDatabaseIntegrationFixture
    {
        public BareRealDatabaseIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}