using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Services.StripeServices.Products;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Stripe;
using Xunit;
using Xunit.Abstractions;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class StripeSubscriptionServiceFixture : StripeServiceFixtureBase
    {
        private StripeSubscriptionService service = null!;
        private StagingProductRegistry registry = null!;

        public StripeSubscriptionServiceFixture(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task ProcessSubscriptionUpdate()
        {
            var subscription = new Subscription()
            {
                // TODO: Configure
            };

            var accountStore = ResolveStore<Account>();
            var handler = new ProcessStripeSubscriptionUpdatedHandler(accountStore, service, registry, Substitute.For<ILogger<ProcessStripeSubscriptionUpdatedHandler>>());
            await Task.CompletedTask;
            // act
            // await handler.Handle(new SubscriptionUpdatedEvent(subscription), CancellationToken.None);

            // assert results of this call
        }

        public override Task DisposeAsync()
        {
            return base.DisposeAsync();
        }

        public override Task InitializeAsync()
        {
            // var stripeService = Container.GetService<ISubscriptionService>();
            // logger = Substitute.For<ILogger<IStripeSubscriptionService>>();
            // service = new StripeSubscriptionService(logger, stripeService);
            // registry = new StagingProductRegistry();
            return base.InitializeAsync();
        }
    }
}