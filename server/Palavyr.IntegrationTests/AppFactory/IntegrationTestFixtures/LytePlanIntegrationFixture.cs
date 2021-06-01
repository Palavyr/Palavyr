using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.DataCreators;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures
{
    public class LytePlanIntegrationFixture : RealDatabaseIntegrationFixture
    {
        public LytePlanIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }


        public override async Task InitializeAsync()
        {
            await this
                .CreateDefaultAccountAndSessionBuilder()
                .WithDefaultPassword()
                .WithDefaultAccountId()
                .WithDefaultAccountType()
                .WithDefaultApiKey()
                .WithDefaultEmailAddress()
                .WithLytePlan()
                .Build();
        }
    }
}