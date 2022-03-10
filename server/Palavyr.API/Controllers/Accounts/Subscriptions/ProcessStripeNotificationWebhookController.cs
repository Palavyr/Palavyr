using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class ProcessStripeNotificationWebhookController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private readonly HttpContextAccessor contextAccessor;
        public const string Route = "payments/payments-webhook";

        public ProcessStripeNotificationWebhookController(IMediator mediator, HttpContextAccessor contextAccessor)
        {
            this.mediator = mediator;
            this.contextAccessor = contextAccessor;
        }

        [AllowAnonymous]
        [HttpPost(Route)]
        public async Task SubscriptionWebhook()
        {
            await mediator.Publish(new ProcessStripeNotificationWebhookNotification());
        }
    }
}