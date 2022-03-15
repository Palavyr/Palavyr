using System;
using System.Threading.Tasks;
using Autofac;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public abstract class StripeServiceFixtureBase : InMemoryIntegrationFixture
    {
        protected StripeServiceFixtureBase(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        public DateTime CreatedAt = DateTime.Now;


        public override async Task InitializeAsync()
        {
            SetAccountIdTransport();
            SetCancellationToken();
            await this.SetupLyteAccount();
            await base.InitializeAsync();
        }
    }
}