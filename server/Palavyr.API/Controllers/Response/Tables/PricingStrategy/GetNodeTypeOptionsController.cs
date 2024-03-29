using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.API.Controllers.Response.Tables.PricingStrategy
{
    public class GetNodeTypeOptionsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "configure-conversations/{intentId}/node-type-options";


        public GetNodeTypeOptionsController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<IEnumerable<NodeTypeOptionResource>> Get([FromRoute] string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetNodeTypeOptionsRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}