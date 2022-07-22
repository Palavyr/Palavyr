using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Services.StripeServices;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.IntegrationTests.Mocks
{
    public class MockStripeWebhookAuthService : IStripeWebhookAuthService
    {
        private readonly IGuidUtils guidUtils;

        public MockStripeWebhookAuthService(IGuidUtils guidUtils)
        {
            this.guidUtils = guidUtils;
        }
        public async Task<(Event eventPayload, string payloadSignature)> AuthenticateWebhookRequest(HttpContext context)
        {
            await Task.Yield();
            
            var e = new Event();
            e.Data = new EventData();
            e.Data.Object = new Session();
            e.Type = Events.CheckoutSessionCompleted;
            
            return (e, guidUtils.CreateNewId());
        }
    }
}