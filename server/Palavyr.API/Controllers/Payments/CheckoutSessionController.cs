using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.API.Controllers.Payments
{
    [Route("api/payments")]
    [ApiController]
    public class CheckoutSessionController : BaseController
    {
        private static ILogger<CheckoutSessionController> _logger;
        private readonly IStripeClient client = new StripeClient(StripeConfiguration.ApiKey);

        public CheckoutSessionController(
            ILogger<CheckoutSessionController> logger,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env
        ) : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;
        }

        [HttpGet("checkout-session")]
        public async Task<IActionResult> CheckoutSession(string sessionId)
        {
            var service = new SessionService(this.client);
            var session = await service.GetAsync(sessionId);
            return Ok(session);
        }
    }
}