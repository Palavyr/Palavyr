using System.Threading.Tasks;
using MediatR;
using Palavyr.Component.ComponentTestBase;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Stripe;
using Xunit;

namespace Palavyr.Component.Tests.StripeWebhookHandlers
{
    public class StripeSubscriptionServiceTest : ComponentTest
    {

        public StripeSubscriptionServiceTest(ComponentClassFixture fixture) : base(fixture)
        {
        
        }

        [Fact]
        public async Task ProcessSubscriptionUpdate()
        {
            var subscription = new Subscription()
            {
                // TODO: Configure
            };

            var handler = ResolveType<INotificationHandler<SubscriptionUpdatedEvent>>();
            await Task.CompletedTask;
        }
    }
}