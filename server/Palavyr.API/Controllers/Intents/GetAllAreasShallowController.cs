using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Intents
{
    public class GetAllAreasShallowController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "areas";
        public GetAllAreasShallowController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<IEnumerable<IntentResource>> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetAllAreasRequest(), cancellationToken);
            return response.Response;
        }
    }
}