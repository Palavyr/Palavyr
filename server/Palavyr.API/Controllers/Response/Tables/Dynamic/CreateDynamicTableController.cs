using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public class CreateDynamicTableController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "tables/dynamic/{intentId}";


        public CreateDynamicTableController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // One controller for getting each table type. A separate call to get the type. Each table has a different
        // structure, and thus a different type. So we can't return multiple types from the same controller without
        // further generalization. This can be done later if its worth it. Adding a new controller for each type
        // isn't that big of a deal since we'll only have dozens of types probably. If we make money, then we can switch
        // to a generic pattern. Its just too complex to implement right now.

        [HttpPost(Route)]
        public async Task<DynamicTableMeta> Create(
            [FromRoute]
            string intentId,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new CreateDynamicTableRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}