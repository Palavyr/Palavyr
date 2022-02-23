using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Services.StripeServices.Products;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Stripe;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeHandlers
{
    public class StripeSubscriptionServiceFixture : BaseIntegrationFixture
    {
        private StripeSubscriptionService service = null!;
        private ILogger<IStripeSubscriptionService> logger = null!;
        private StagingProductRegistry registry = null!;

        public StripeSubscriptionServiceFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task ProcessSubscriptionUpdate()
        {
            var subscription = new Subscription()
            {
                // TODO: Configure
            };

            var handler = new ProcessStripeSubscriptionUpdatedHandler(AccountsContext, service, registry, Substitute.For<ILogger<ProcessStripeSubscriptionUpdatedHandler>>());

            // act
            await handler.Handle(new SubscriptionUpdatedEvent(subscription), CancellationToken.None);
            
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