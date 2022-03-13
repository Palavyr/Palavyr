using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests
{
    public class WhenAttemptingToResolveAllTypes : InMemoryIntegrationFixture
    {
        [Fact]
        public void AllTypesAreResolvedSuccessfully()
        {
            var registrations = Container.LifetimeScope.ComponentRegistry.Registrations;
            foreach (var rego in registrations)
            {
                var instance = Container.GetService(rego.GetType());
            }
        }

        public WhenAttemptingToResolveAllTypes(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory)
            : base(testOutputHelper, factory)
        {
        }
    }
}