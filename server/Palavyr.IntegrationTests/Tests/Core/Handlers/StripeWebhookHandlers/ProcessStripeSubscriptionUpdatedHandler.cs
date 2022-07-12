using System.Threading.Tasks;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Services.StripeServices.Products;
using Palavyr.Core.Sessions;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Stripe;
using Xunit;
using Xunit.Abstractions;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class StripeSubscriptionServiceTest : StripeServiceTestBaseBase
    {
        private StripeSubscriptionService service = null!;
        private StagingProductRegistry registry = null!;

        public StripeSubscriptionServiceTest(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task ProcessSubscriptionUpdate()
        {
            var subscription = new Subscription()
            {
                // TODO: Configure
            };

            var accountGetter = new StripeWebhookAccountGetter(ResolveStore<Account>(), ResolveType<IAccountIdTransport>());

            var handler = new ProcessStripeSubscriptionUpdatedHandler(accountGetter, service);
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