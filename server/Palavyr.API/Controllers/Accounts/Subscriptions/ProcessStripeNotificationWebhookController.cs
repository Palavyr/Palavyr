using System.Security.Authentication;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.StripeServices;
using Stripe;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    [Route("api")]
    [ApiController]
    public class ProcessStripeNotificationWebhookController : ControllerBase
    {
        private ILogger<ProcessStripeNotificationWebhookController> logger;
        private readonly IStripeClient stripeClient;
        private IConfiguration configuration;
        private AccountsContext accountsContext;
        private IStripeWebhookAuthService stripeWebhookAuthService;
        private IStripeEventWebhookService stripeEventWebhookService;

        public ProcessStripeNotificationWebhookController(
            IConfiguration configuration,
            ILogger<ProcessStripeNotificationWebhookController> logger,
            AccountsContext accountsContext,
            IStripeWebhookAuthService stripeWebhookAuthService,
            IStripeEventWebhookService stripeEventWebhookService
        )
        {
            this.logger = logger;
            this.configuration = configuration;
            this.accountsContext = accountsContext;
            this.stripeClient = new StripeClient(StripeConfiguration.ApiKey);
            this.stripeWebhookAuthService = stripeWebhookAuthService;
            this.stripeEventWebhookService = stripeEventWebhookService;
        }

        /// HOW TO USE STRIPE CLI WITH THIS SERVER
        /// TO EXECUTE WEBHOOK
        /// 1. stripe login
        /// 2. stripe listen --forward-to https://localhost:5001/api/payments/payments-webhook --skip-verify
        /// 3. stripe trigger customer.subscription.created
        ///
        /// This will forward requests to the server via the cli tool.
        /// The way this works:
        /// 1. Request is made either via stripe.com (from my dashboard when making a subscription or from the stripe dashboard) or via the CLI
        /// 2. The request is forwarded to the stripe CLI
        /// 3. The ClI then forwards this to the server
        /// 4. The server is running https, so we need to indicate the above url (https://localhost:5001)
        [AllowAnonymous]
        [HttpPost("payments/payments-webhook")]
        public async Task<IActionResult> SubscriptionWebhook()
        {
            var stripeEvent = await stripeWebhookAuthService.AuthenticateWebhookRequest(HttpContext);
            if (stripeEvent == null)
            {
                throw new AuthenticationException("Stripe webhook request not authenticated.");
            }
            // this does not return bad requests to stripe. We are either successful, or we log errors and throw
            await stripeEventWebhookService.ProcessStripeEvent(stripeEvent);
            return Ok();
        }
    }
}