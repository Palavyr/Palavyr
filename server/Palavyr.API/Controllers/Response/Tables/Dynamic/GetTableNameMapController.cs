using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public class GetTableNameMapController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "tables/dynamic/table-name-map";

        public GetTableNameMapController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<Dictionary<string, string>> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetTableNameMapRequest(), cancellationToken);
            return response.Response;
        }
    }
}