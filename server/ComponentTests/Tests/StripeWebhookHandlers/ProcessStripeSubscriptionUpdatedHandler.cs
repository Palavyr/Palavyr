using System.Threading.Tasks;
using Component.ComponentTestBase;
using MediatR;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Stripe;
using Xunit;

namespace Component.Tests.StripeWebhookHandlers
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