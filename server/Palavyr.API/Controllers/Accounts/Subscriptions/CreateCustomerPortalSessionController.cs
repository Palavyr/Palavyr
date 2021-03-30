using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.BillingPortal;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class CustomerPortalRequest
    {
        public string CustomerId { get; set; }
        public string ReturnUrl { get; set; }
    }

    // TODO
    /// <summary>
    /// Used to update customer billing information - card numbers, etc
    /// </summary>
    public class CreateCustomerPortalSessionController : PalavyrBaseController
    {
        private ILogger<CreateCustomerPortalSessionController> logger;
        private readonly IStripeClient stripeClient;

        public CreateCustomerPortalSessionController(ILogger<CreateCustomerPortalSessionController> logger)
        {
            this.stripeClient = new StripeClient(StripeConfiguration.ApiKey);
            this.logger = logger;
        }

        [HttpPost("payments/customer-portal")]
        public async Task<string> Create([FromHeader] string accountId, [FromBody] CustomerPortalRequest request)
        {
            var options = new SessionCreateOptions()
            {
                Customer = request.CustomerId,
                ReturnUrl = request.ReturnUrl,
            };
            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}