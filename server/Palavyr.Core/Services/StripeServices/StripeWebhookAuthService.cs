using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;
using Stripe;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeWebhookAuthService
    {
        Task<(Event eventPayload, string payloadSignature)> AuthenticateWebhookRequest(HttpContext context);
    }

    public class StripeWebhookAuthService : IStripeWebhookAuthService
    {
        private readonly IAccountRepository accountRepository;
        private ILogger<StripeWebhookAuthService> logger;
        private IConfiguration configuration;
        private const string StripeSignature = "Stripe-Signature";
        private readonly string webhookKeySection = "Stripe:WebhookKey";

        public StripeWebhookAuthService(
            IAccountRepository accountRepository,
            ILogger<StripeWebhookAuthService> logger,
            IConfiguration configuration
        )
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<(Event eventPayload, string payloadSignature)> AuthenticateWebhookRequest(HttpContext context)
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

            try
            {
                var webhookSecret = configuration.GetSection(webhookKeySection).Value;

                var signatureItems = context.Request.Headers[StripeSignature]
                    .ToString()
                    .Trim()
                    .Split(',')
                    .Select(item => item.Trim().Split(new[] { '=' }, 2))
                    .ToDictionary(item => item[0], item => item[1]);

                var payloadSignature = $"{signatureItems["t"]}.{signatureItems["v1"]}";

                var eventPayload = EventUtility.ConstructEvent(
                    requestBody,
                    context.Request.Headers[StripeSignature],
                    webhookSecret,
                    tolerance: 150
                );

                if (await accountRepository.SignedStripePayloadExists(payloadSignature))
                {
                    throw new Exception($"Webhook already processed and recorded in the Webhook Records Table: {eventPayload.Data}");
                }

                return (eventPayload, payloadSignature);
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

                return (null, null);
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Webhook Failed: {ex.Message}");
                return (null, null);
            }
        }
    }
}