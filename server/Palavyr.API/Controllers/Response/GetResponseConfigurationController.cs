using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response
{
    public class GetResponseConfigurationController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "response/configuration/{intentId}";

        public GetResponseConfigurationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<Area> Get([FromRoute] string intentId, CancellationToken cancellationToken)
        {
            
            var response = await mediator.Send(new GetResponseConfigurationRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}