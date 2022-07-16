using System.Threading.Tasks;
using Component.ComponentTestBase;
using NSubstitute;
using Palavyr.Core.Data;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;
using Stripe;
using Test.Common.Builders.Accounts;
using Xunit;
using Account = Palavyr.Core.Data.Entities.Account;

namespace Component.Tests.StripeWebhookHandlers
{
    public class ProcessStripePaymentMethodUpdatedHandlerTest : ComponentTest
    {
        public ProcessStripePaymentMethodUpdatedHandlerTest(ComponentClassFixture fixture) : base(fixture)
        {
            TransportsEnabled = false;
        }

        [Fact]
        public async Task HandlesPaymentMethodUpdate()
        {
            await new AccountObjectBuilder()
                .WithAccountId(AccountId)
                .WithStripeCustomerId(StripeCustomerId)
                .BuildAndMakeRaw(ResolveType<AppDataContexts>(), CancellationToken);

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