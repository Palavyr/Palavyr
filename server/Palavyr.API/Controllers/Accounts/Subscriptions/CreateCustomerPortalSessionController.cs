using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Services.StripeServices;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class CustomerPortalRequest
    {
        public string CustomerId { get; set; }
        public string ReturnUrl { get; set; }
    }
    
    public class CreateCustomerPortalSessionController : PalavyrBaseController
    {
        private readonly IStripeCustomerManagementPortalService stripeCustomerManagementPortalService;

        public CreateCustomerPortalSessionController(IStripeCustomerManagementPortalService stripeCustomerManagementPortalService)
        {
            this.stripeCustomerManagementPortalService = stripeCustomerManagementPortalService;
        }

        [HttpPost("payments/customer-portal")]
        public async Task<string> Create([FromHeader] string accountId, [FromBody] CustomerPortalRequest request)
        {
            var portalUrl = await stripeCustomerManagementPortalService.FormCustomerSubscriptionManagementPortalUrl(request.CustomerId, request.ReturnUrl);
            return portalUrl;
        }
    }
}