using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Response
{
    public class GetResponseConfigurationController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public GetResponseConfigurationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(GetResponseConfigurationRequest.Route)]
        public async Task<IntentResource> Get([FromRoute] string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetResponseConfigurationRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}