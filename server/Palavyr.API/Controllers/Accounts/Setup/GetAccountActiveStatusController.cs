using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Accounts.Setup
{
    public class GetAccountActiveStatusController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/is-active";


        public GetAccountActiveStatusController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<bool> CheckIsActive(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetAccountActiveStatusRequest(), cancellationToken);
            return response.Response;
        }
    }
}