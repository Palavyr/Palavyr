using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.DynamicTableService;

// you need to write 6 more mappers at the moment. can you change the api so that you dont have to use one of the mapers?
namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    [ApiController]
    public abstract class PricingStrategyControllerBase<TEntity, TResource>
        : PalavyrBaseController, IDynamicTableController<TEntity, TResource>
        where TEntity : class, IDynamicTable<TEntity>, new()
        where TResource : IPricingStrategyTableRowResource, new()
    {
        private readonly IMediator mediator;

        public PricingStrategyControllerBase(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpDelete("intent/{intentId}/table/{tableId}")]
        public async Task Delete([FromRoute] string intentId, [FromRoute] string tableId, CancellationToken cancellationToken)
        {
            await mediator.Publish(new DeletePricingStrategyTableRequest<TEntity, TResource>() { IntentId = intentId, TableId = tableId }, cancellationToken);
        }


        [HttpPut("intent/{intentId}/table/{tableId}")]
        public async Task<IEnumerable<TResource>> SaveDynamicTable([FromRoute] string intentId, [FromRoute] string tableId, [FromBody] DynamicTable<TEntity> dynamicTable)
        {
            var resource = await mediator.Send(
                new SavePricingStrategyTableRequest<TEntity, TResource>()
                {
                    TableId = tableId,
                    IntentId = intentId
                });
            return resource.Resource;
        }


        [HttpGet("intent/{intentId}/table/{tableId}")]
        public async Task<DynamicTableDataResource<TResource>> GetPricingStrategyTableRows([FromRoute] string intentId, [FromRoute] string tableId)
        {
            var response = await mediator.Send(
                new GetPricingStrategyTableRowsRequest<TEntity, TResource>()
                {
                    IntentId = intentId,
                    TableId = tableId
                });
            return response.Resource;
        }

        [HttpGet("intent/{intentId}/table/{tableId}/template")]
        public async Task<TResource> GetDynamicRowTemplate([FromRoute] string intentId, [FromRoute] string tableId)
        {
            var response = await mediator.Send(new GetPricingStrategyTableRowTemplateRequest<TEntity, TResource>());
            return response.Resource;
        }
    }
}