using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class GetCustomerIdController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "payments/customer-id";

        public GetCustomerIdController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetStripeCustomerIdRequest(), cancellationToken);
            return response.Response;
        }
    }
}