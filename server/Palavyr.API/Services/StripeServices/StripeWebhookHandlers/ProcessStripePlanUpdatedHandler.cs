using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.API.Services.StripeServices.StripeWebhookHandlers
{
    public class ProcessStripePlanUpdatedHandler
    {
        private readonly ILogger<ProcessStripePlanUpdatedHandler> logger;

        public ProcessStripePlanUpdatedHandler(ILogger<ProcessStripePlanUpdatedHandler> logger)
        {
            this.logger = logger;
        }

        public void ProcessStripePlanUpdate(Plan planUpdate)
        {
            logger.LogDebug("Plan Successfully Updated");
            logger.LogDebug(planUpdate.Object);
        }
    }
}