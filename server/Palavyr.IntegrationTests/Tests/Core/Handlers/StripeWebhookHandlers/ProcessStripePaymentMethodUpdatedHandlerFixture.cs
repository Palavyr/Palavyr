using System.Threading.Tasks;
using NSubstitute;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Stripe;
using Xunit;
using Xunit.Abstractions;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripePaymentMethodUpdatedHandlerFixture : StripeServiceFixtureBase
    {
        public ProcessStripePaymentMethodUpdatedHandlerFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task HandlesPaymentMethodUpdate()
        {
            var paymentMethod = new PaymentMethod()
            {
                CustomerId = StripeCustomerId,
                Livemode = false
            };

            var mockClient = Substitute.For<ISesEmail>();
            var @event = new PaymentMethodUpdatedEvent(paymentMethod);
            var handler = new ProcessStripePaymentMethodUpdatedHandler(mockClient, ResolveStore<Account>());

            await handler.Handle(@event, CancellationToken);

            await mockClient.ReceivedWithAnyArgs().SendEmail(default!, default!, default!, default!, default!);
        }
    }
}