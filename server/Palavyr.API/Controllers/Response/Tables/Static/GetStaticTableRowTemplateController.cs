using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    public class GetStaticTableRowTemplateController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "response/configuration/{intentId}/static/tables/{tableId}/row/template";

        public GetStaticTableRowTemplateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<StaticTableRow> Get(
            [FromRoute]
            string intentId,
            [FromRoute]
            string tableId,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetStaticTableRowTemplateRequest(intentId, tableId), cancellationToken);
            return response.Response;
        }
    }
}