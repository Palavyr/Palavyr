using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Stripe.Checkout;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class GetStripeCheckoutSessionController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "payments/checkout-session";

        public GetStripeCheckoutSessionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<Session> Get(string sessionId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetStripeCheckoutSessionRequest() { SessionId = sessionId }, cancellationToken);
            return response.Response;
        }
    }
}