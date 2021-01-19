using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.API.Services.StripeServices
{
    public interface IStripeWebhookAuthService
    {
        Task<Event> AuthenticateWebhookRequest(HttpContext context);
    }
    
    public class StripeWebhookAuthService : IStripeWebhookAuthService
    {
        private ILogger<StripeWebhookAuthService> logger;
        private IConfiguration configuration;
        private const string stripeSignature = "Stripe-Signature";
        private readonly string webhookKeySection = "Stripe:WebhookKey";

        public StripeWebhookAuthService(
            ILogger<StripeWebhookAuthService> logger,
            IConfiguration configuration
            )
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<Event> AuthenticateWebhookRequest(HttpContext context)
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

            try
            {
                var webhookSecret = configuration.GetSection(webhookKeySection).Value;
                return EventUtility.ConstructEvent(
                    requestBody,
                    context.Request.Headers[stripeSignature],
                    webhookSecret
                );
            }
            catch (NullReferenceException ex)
            {
                if (string.IsNullOrWhiteSpace(requestBody))
                {
                    logger.LogDebug("Webhook failed: request body was empty");
                }
                else
                {
                    logger.LogDebug("Webhook failed: HttpContext ");
                }

                return null;
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Webhook Failed: {ex.Message}");
                return null;
            }
        }
        
    }
}