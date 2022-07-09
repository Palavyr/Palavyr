using System.Threading.Tasks;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores.StoreExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class NewStripeEventReceivedEventHandlerFixture : StripeServiceFixtureBase
    {
        public NewStripeEventReceivedEventHandlerFixture(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task HandlesStripeEventReceivedEvent_WhenExists()
        {
            var signature = A.RandomId();
            var stripeWebhookStore = ResolveStore<StripeWebhookReceivedRecord>();

            await stripeWebhookStore.AddStripeEvent(A.RandomId(), signature);
            var @event = new NewStripeEventReceivedEvent(signature);
            var handler = new NewStripeEventReceivedEventHandler(stripeWebhookStore);

            var result = await handler.Handle(@event, CancellationToken);

            result.ShouldCancelProcessing.ShouldBeTrue();
        }

        [Fact]
        public async Task HandlesNewStripeEventReceivedEvent()
        {
            var signature = A.RandomId();
            var @event = new NewStripeEventReceivedEvent(signature);
            var stripeWebhookStore = ResolveStore<StripeWebhookReceivedRecord>();
            var handler = new NewStripeEventReceivedEventHandler(stripeWebhookStore);

            var result = await handler.Handle(@event, CancellationToken);

            result.ShouldCancelProcessing.ShouldBeFalse();
        }
    }
}