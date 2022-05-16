using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.Meta
{
    public class GetDynamicTableMetasController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "tables/dynamic/metas/{intentId}";


        public GetDynamicTableMetasController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<DynamicTableMetaResource[]> Get(string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetDynamicTableMetasRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}