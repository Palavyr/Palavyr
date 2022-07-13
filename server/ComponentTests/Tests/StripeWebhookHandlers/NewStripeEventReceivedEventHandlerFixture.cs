using System.Threading.Tasks;
using Component.AppFactory.ComponentTestBase.BaseFixture;
using MediatR;
using Palavyr.Core.Data;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores.StoreExtensionMethods;
using Shouldly;
using Test.Common.Builders.Accounts;
using Test.Common.Random;
using Xunit;

namespace Component.Tests.StripeWebhookHandlers
{
    public class NewStripeEventReceivedEventHandlerTest : ComponentTest
    {
        public NewStripeEventReceivedEventHandlerTest(ComponentClassFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task HandlesStripeEventReceivedEvent_WhenExists()
        {
            var customerId = A.RandomId();

            var account = await new AccountObjectBuilder()
                .WithAccountId(AccountId)
                .WithStripeCustomerId(customerId)
                .BuildAndMakeRaw(ResolveType<AccountsContext>(), CancellationToken);

            
            var signature = A.RandomId();
            var stripeWebhookStore = ResolveStore<StripeWebhookReceivedRecord>();

            await stripeWebhookStore.AddStripeEvent(A.RandomId(), signature);
            var @event = new NewStripeEventReceivedEvent(signature);

            var handler = ResolveType<IRequestHandler<NewStripeEventReceivedEvent, NewStripeEventReceivedEventResponse>>();

            var result = await handler.Handle(@event, CancellationToken);

            result.ShouldCancelProcessing.ShouldBeTrue();
        }

        [Fact]
        public async Task HandlesNewStripeEventReceivedEvent()
        {

            var @event = new NewStripeEventReceivedEvent(A.RandomId());

            var handler = ResolveType<IRequestHandler<NewStripeEventReceivedEvent, NewStripeEventReceivedEventResponse>>();
            var result = await handler.Handle(@event, CancellationToken);

            result.ShouldCancelProcessing.ShouldBeFalse();
        }
    }
}