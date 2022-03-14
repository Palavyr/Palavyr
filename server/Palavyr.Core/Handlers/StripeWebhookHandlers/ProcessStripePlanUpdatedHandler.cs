using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripePlanUpdatedHandler : INotificationHandler<PlanUpdatedEvent>
    {
        private readonly ILogger<ProcessStripePlanUpdatedHandler> logger;

        public ProcessStripePlanUpdatedHandler(ILogger<ProcessStripePlanUpdatedHandler> logger)
        {
            this.logger = logger;
        }

        public async Task Handle(PlanUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var planUpdate = notification.plan;
            logger.LogDebug("Plan Successfully Updated");
            logger.LogDebug(planUpdate.Object);
        }
    }

    public class PlanUpdatedEvent : INotification
    {
        public readonly Plan plan;

        public PlanUpdatedEvent(Plan plan)
        {
            this.plan = plan;
        }
    }
}