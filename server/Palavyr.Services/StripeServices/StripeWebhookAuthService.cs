using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Services.DatabaseService;
using Stripe;

namespace Palavyr.Services.StripeServices
{
    public class StripeWebhookAuthService
    {
        private readonly IAccountsConnector accountsConnector;
        private ILogger<StripeWebhookAuthService> logger;
        private IConfiguration configuration;
        private const string stripeSignature = "Stripe-Signature";
        private readonly string webhookKeySection = "Stripe:WebhookKey";

        public StripeWebhookAuthService(
            IAccountsConnector accountsConnector,
            ILogger<StripeWebhookAuthService> logger,
            IConfiguration configuration
            )
        {
            this.accountsConnector = accountsConnector;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<Event> AuthenticateWebhookRequest(HttpContext context)
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

            try
            {
                var webhookSecret = configuration.GetSection(webhookKeySection).Value;
                
                var signatureItems = context.Request.Headers[stripeSignature]
                    .ToString()
                    .Trim()
                    .Split(',')
                    .Select(item => item.Trim().Split(new[] { '=' }, 2))
                    .ToDictionary(item => item[0], item => item[1]);

                var signedPayload = $"{signatureItems["t"]}.{signatureItems["v1"]}";
                
                var eventPayload = EventUtility.ConstructEvent(
                    requestBody,
                    context.Request.Headers[stripeSignature],
                    webhookSecret,
                    tolerance: 150
                );
                
                if (accountsConnector.SignedStripePayloadExists(signedPayload))
                {
                    throw new Exception($"Webhook already processed and recorded in the Webhook Records Table: {eventPayload.Data}");
                }

                return eventPayload;
            }
            catch (NullReferenceException ex)
            {
                if (string.IsNullOrWhiteSpace(requestBody))
                {
                    logger.LogDebug($"Webhook failed: request body was empty: {ex.Message}");
                }
                else
                {
                    logger.LogDebug($"Webhook failed: HttpContext: {ex.Message}");
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