using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.API.Controllers.Payments
{
    public class CustomerPortalRequest
    {
        public string SessionId { get; set; }
        public string ReturnUrl { get; set; }
    }
    
    // TODO
    /// <summary>
    /// Used to update customer billing information - card numbers, etc
    /// </summary>
    [Route("api")]
    [ApiController]
    public class CreateCustomerPortalSessionController : ControllerBase
    {
        private ILogger<CreateCustomerPortalSessionController> logger;
        private readonly IStripeClient stripeClient;

        public CreateCustomerPortalSessionController(ILogger<CreateCustomerPortalSessionController> logger)
        {
            this.stripeClient = new StripeClient(StripeConfiguration.ApiKey);
            this.logger = logger;
        }

        [HttpPost("payments/customer-portal")]
        public async Task<IActionResult> Create([FromBody] CustomerPortalRequest request)
        {
            var checkoutSessionId = request.SessionId;
            var checkoutService = new SessionService(stripeClient);
            var checkoutSession = await checkoutService.GetAsync(checkoutSessionId);
            
            var options = new Stripe.BillingPortal.SessionCreateOptions()
            {
                Customer = checkoutSession.CustomerId,
                ReturnUrl = request.ReturnUrl
            };

            var service = new Stripe.BillingPortal.SessionService(stripeClient);
            var session = await service.CreateAsync(options);

            return Ok(session);
        }
    }
}