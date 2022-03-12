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


        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            return base.CustomizeContainer(builder);
        }


        public override Task InitializeAsync()
        {
            SetAccountIdTransport();
            SetCancellationToken();
            this.SetupLyteAccount();
            return base.InitializeAsync();
        }
    }
}