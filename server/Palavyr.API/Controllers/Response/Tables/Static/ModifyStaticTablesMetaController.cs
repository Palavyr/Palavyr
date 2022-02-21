using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    public class ModifyStaticTablesMetaController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "response/configuration/static/tables/save";

        public ModifyStaticTablesMetaController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<List<StaticTablesMeta>> Modify(
            ModifyStaticTablesMetaRequest request,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}