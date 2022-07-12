using System.Threading.Tasks;
using NSubstitute;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Stripe;
using Xunit;
using Xunit.Abstractions;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripePaymentMethodUpdatedHandlerTest : StripeServiceTestBaseBase
    {
        public ProcessStripePaymentMethodUpdatedHandlerTest(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
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
            var accountGetter = new StripeWebhookAccountGetter(ResolveStore<Account>(), ResolveType<IAccountIdTransport>());

            var handler = new ProcessStripePaymentMethodUpdatedHandler(mockClient, accountGetter);

            await handler.Handle(@event, CancellationToken);

            await mockClient.ReceivedWithAnyArgs().SendEmail(default!, default!, default!, default!, default!);
        }
    }
}