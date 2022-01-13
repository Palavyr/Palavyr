using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class ProcessStripeNotificationWebhookController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "payments/payments-webhook";

        public ProcessStripeNotificationWebhookController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost(Route)]
        public async Task SubscriptionWebhook()
        {
            await mediator.Publish(new ProcessStripeNotificationWebhookNotification());
        }
    }
}