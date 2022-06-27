using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetApiKeyController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public GetApiKeyController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(GetApiKeyRequest.Route)]
        public async Task<string> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetApiKeyRequest(), cancellationToken);
            return response.Response;
        }
    }
}