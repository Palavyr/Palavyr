using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.StripeServices;
using Stripe;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{

    public class ProcessStripeNotificationWebhookController : PalavyrBaseController
    {
        private ILogger<ProcessStripeNotificationWebhookController> logger;
        private StripeWebhookAuthService stripeWebhookAuthService;
        private StripeEventWebhookService stripeEventWebhookService;

        public ProcessStripeNotificationWebhookController(
            ILogger<ProcessStripeNotificationWebhookController> logger,
            StripeWebhookAuthService stripeWebhookAuthService,
            StripeEventWebhookService stripeEventWebhookService
        )
        {
            this.logger = logger;
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
        /// 2. The request is forwarded to the stripe CLI WOOT
        /// 3. The ClI then forwards this to the server
        /// 4. The server is running https, so we need to indicate the above url (https://localhost:5001)
        [AllowAnonymous]
        [HttpPost("payments/payments-webhook")]
        public async Task<IActionResult> SubscriptionWebhook()
        {
            var stripeEvent = await stripeWebhookAuthService.AuthenticateWebhookRequest(HttpContext);
            if (stripeEvent == null)
            {
                logger.LogDebug("Stripe webhook authentication failed. Check that you are using the correct webhook auth key.");
                throw new AuthenticationException("Stripe webhook request not authenticated.");
            }
            
            // this does not return bad requests to stripe. We are either successful, or we log errors and throw.
            // If stripe does not receive a 200 response, then it will retry.
            // https://stripe.com/docs/billing/subscriptions/webhooks
            try
            {
                logger.LogDebug($"Trying to process a stripe event: {stripeEvent.Type}");
                await stripeEventWebhookService.ProcessStripeEvent(stripeEvent);
                return Ok();
            }
            catch (StripeException ex)
            {
                logger.LogDebug($"Stripe error: {ex.Message}");
                return BadRequest();
            }
        }
    }
}