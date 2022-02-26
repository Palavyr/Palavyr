using System.Threading.Tasks;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class NewStripeEventReceivedEventHandlerFixture : StripeServiceFixtureBase
    {
        public NewStripeEventReceivedEventHandlerFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task HandlesStripeEventReceivedEvent_WhenExists()
        {
            var signature = A.RandomId();
            await AccountRepository.AddStripeEvent(A.RandomId(), signature);
            var @event = new NewStripeEventReceivedEvent(signature);
            var handler = new NewStripeEventReceivedEventHandler(AccountRepository);

            var result = await handler.Handle(@event, CancellationToken);

            result.ShouldCancelProcessing.ShouldBeTrue();
        }

        [Fact]
        public async Task HandlesNewStripeEventReceivedEvent()
        {
            var signature = A.RandomId();
            var @event = new NewStripeEventReceivedEvent(signature);
            var handler = new NewStripeEventReceivedEventHandler(AccountRepository);

            var result = await handler.Handle(@event, CancellationToken);

            result.ShouldCancelProcessing.ShouldBeFalse();
        }
    }
}