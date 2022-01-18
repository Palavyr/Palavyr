using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class CreateCheckoutSessionController : PalavyrBaseController
    {
        public const string Route = "checkout/create-checkout-session";
        private readonly IMediator mediator;

        public CreateCheckoutSessionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<string> Post(
            [FromBody]
            CreateStripeCheckoutSessionRequest request,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}