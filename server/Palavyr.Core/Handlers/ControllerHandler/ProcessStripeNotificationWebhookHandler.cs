using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.StripeServices;
using Stripe;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ProcessStripeNotificationWebhookHandler : IRequestHandler<ProcessStripeNotificationWebhookRequest, ProcessStripeNotificationWebhookResponse>
    {
        private readonly ILogger<ProcessStripeNotificationWebhookHandler> logger;
        private readonly IStripeWebhookAuthService stripeWebhookAuthService;
        private readonly IStripeEventWebhookRoutingService stripeEventWebhookRoutingService;

        public ProcessStripeNotificationWebhookHandler(
            ILogger<ProcessStripeNotificationWebhookHandler> logger,
            IStripeWebhookAuthService stripeWebhookAuthService,
            IStripeEventWebhookRoutingService stripeEventWebhookRoutingService
        )
        {
            this.logger = logger;
            this.stripeWebhookAuthService = stripeWebhookAuthService;
            this.stripeEventWebhookRoutingService = stripeEventWebhookRoutingService;
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
        public async Task<ProcessStripeNotificationWebhookResponse> Handle(ProcessStripeNotificationWebhookRequest request, CancellationToken cancellationToken)
        {
            var httpContext = request.HttpContext;
            var (stripeEvent, signature) = await stripeWebhookAuthService.AuthenticateWebhookRequest(httpContext);
            
            
            if (stripeEvent == null)
            {
                logger.LogDebug("Stripe webhook authentication failed. Check that you are using the correct webhook auth key");
                throw new AuthenticationException("Stripe webhook request not authenticated.");
            }

            // this does not return bad requests to stripe. We are either successful, or we log errors and throw.
            // If stripe does not receive a 200 response, then it will retry.
            // https://stripe.com/docs/billing/subscriptions/webhooks
            try
            {
                logger.LogDebug("Trying to process a stripe event: {StripeEventType}", stripeEvent.Type);
                await stripeEventWebhookRoutingService.ProcessStripeEvent(stripeEvent, signature, cancellationToken);
                return new ProcessStripeNotificationWebhookResponse();
            }
            catch (StripeException ex)
            {
                logger.LogDebug("Stripe error: {StripeError}", ex.Message);
                throw new DomainException(ex.Message);
            }
        }
    }


    public class ProcessStripeNotificationWebhookRequest : IRequest<ProcessStripeNotificationWebhookResponse>
    {
        public HttpContext HttpContext { get; }
        public const string Route = "payments/payments-webhook";

        public ProcessStripeNotificationWebhookRequest(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }
    }

    public class ProcessStripeNotificationWebhookResponse
    {
        
    }
}