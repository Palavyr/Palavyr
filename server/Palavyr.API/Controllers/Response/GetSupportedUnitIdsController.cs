using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Services.Units;

namespace Palavyr.API.Controllers.Response
{
    public class GetSupportedUnitIdsController : PalavyrBaseController
    {
        public const string Route = "configuration/unit-types";
        private readonly IMediator mediator;

        public GetSupportedUnitIdsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<List<QuantUnit>> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetSupportedUnitIdsRequest(), cancellationToken);
            return response.Response;
        }
    }
}