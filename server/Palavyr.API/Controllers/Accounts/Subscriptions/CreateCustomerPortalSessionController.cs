using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class CreateCustomerPortalSessionController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "payments/customer-portal";

        public CreateCustomerPortalSessionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<string> Create([FromBody] CreateCustomerPortalSessionRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}