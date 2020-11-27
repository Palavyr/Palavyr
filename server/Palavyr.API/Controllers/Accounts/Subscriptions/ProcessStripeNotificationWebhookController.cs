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
    [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("payments/payments-webhook")]
        public async Task<IActionResult> SubscriptionWebhook()
        {
            var stripeEvent = await stripeWebhookAuthService.AuthenticateWebhookRequest(HttpContext);
            
            // this does not return bad requests to stripe. We are either successful, or we log errors and throw
            await stripeEventWebhookService.ProcessStripeEvent(stripeEvent);
            return Ok();
        }
    }
}