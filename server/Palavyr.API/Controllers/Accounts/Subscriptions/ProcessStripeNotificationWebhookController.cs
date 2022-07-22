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
        private readonly IHttpContextAccessor httpContextAccessor;


        public ProcessStripeNotificationWebhookController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            this.httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost(ProcessStripeNotificationWebhookRequest.Route)]
        public async Task SubscriptionWebhook()
        {
            await mediator.Send(new ProcessStripeNotificationWebhookRequest(httpContextAccessor.HttpContext));
        }
    }
}