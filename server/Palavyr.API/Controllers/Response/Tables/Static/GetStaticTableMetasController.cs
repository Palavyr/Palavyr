using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    [Obsolete("It seems this is no longer used by the frontend")]
    public class GetStaticTableMetasController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "response/configuration/{intentId}/static/tables";

        public GetStaticTableMetasController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<IEnumerable<StaticTablesMetaResource>> GetStaticTablesMetas(string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetStaticTableMetasRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}