using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Areas
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
        public async Task<List<Area>> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetAllAreasRequest(), cancellationToken);
            return response.Response;
        }
    }
}