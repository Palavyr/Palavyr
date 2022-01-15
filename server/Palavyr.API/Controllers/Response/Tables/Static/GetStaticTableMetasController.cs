using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    public class GetStaticTableMetasController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "response/configuration/{intentId}/static/tables";

        public GetStaticTableMetasController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<List<StaticTablesMeta>> GetStaticTablesMetas(string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetStaticTableMetasRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}