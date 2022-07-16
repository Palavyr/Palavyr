using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.Meta
{
    public class ModifyDynamicTableMetaController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "tables/dynamic/modify";

        public ModifyDynamicTableMetaController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<PricingStrategyTableMeta> Modify([FromBody] ModifyDynamicTableMetaRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}