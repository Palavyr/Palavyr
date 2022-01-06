using System.Threading.Tasks;
using Palavyr.API.Controllers.Accounts.Settings;
using Palavyr.Core.Exceptions;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.Accounts.Settings
{
    public class WhenGettingAnApiKeyForAnAccountThatDoesNotExist : BareInMemoryIntegrationFixture
    {
        private const string Route = GetApiKeyController.Uri;

        public WhenGettingAnApiKeyForAnAccountThatDoesNotExist(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }


        [Fact]
        public async Task GetApiKeyFails()
        {
            Should.ThrowAsync<DomainException>(
                async () => { await Client.GetAsync(Route); });
        }
    }
}