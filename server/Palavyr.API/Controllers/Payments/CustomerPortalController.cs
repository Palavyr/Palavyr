using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.API.Controllers.Payments
{
    public class CustomerPortalRequest
    {
        public string SessionId { get; set; }
    }

    [Route("api/payments")]
    [ApiController]
    public class CustomerPortalController : BaseController
    {
        private static ILogger<CustomerPortalController> _logger;
        private readonly IStripeClient client = new StripeClient();
        
        public CustomerPortalController(
            ILogger<CustomerPortalController> logger,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env
        ) : base(accountContext, convoContext, dashContext, env)
        {
            StripeConfiguration.ApiKey = "sk_test_51HOtDQAnPqY603aZg1LhzHge6qQ7AEYcGPQhhCqMc5gXwfyr6XTEJJvJisBtzhFChIeOnytjCkhHK2ZmEgIuWyup00loOlq4W1";
            _logger = logger;
        }

        [HttpPost("customer-portal")]
        public async Task<IActionResult> CustomerPortal([FromBody] CustomerPortalRequest request)
        {
            var checkoutSessionId = request.SessionId;
            var checkoutService = new SessionService(this.client);
            var checkoutSession = await checkoutService.GetAsync(checkoutSessionId);

            var returnUrl = "https://localhost:5001/dashboard/subscription/";

            var options = new Stripe.BillingPortal.SessionCreateOptions()
            {
                Customer = checkoutSession.CustomerId,
                ReturnUrl = returnUrl,
            };
            
            var service = new Stripe.BillingPortal.SessionService(this.client);
            var session = await service.CreateAsync(options);

            return Ok(session);

        }
        
    }
}