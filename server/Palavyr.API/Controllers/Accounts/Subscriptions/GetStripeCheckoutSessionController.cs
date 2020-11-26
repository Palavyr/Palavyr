using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    [Route("api")]
    [ApiController]
    public class GetStripeCheckoutSessionController : ControllerBase
    {
        private ILogger<GetStripeCheckoutSessionController> logger;
        private readonly IStripeClient client = new StripeClient(StripeConfiguration.ApiKey);

        public GetStripeCheckoutSessionController(ILogger<GetStripeCheckoutSessionController> logger) 
        {
            this.logger = logger;
        }

        [HttpGet("payments/checkout-session")]
        public async Task<Session> Get(string sessionId)
        {
            var service = new SessionService(this.client);
            var session = await service.GetAsync(sessionId);
            return session;
        }
    }
}