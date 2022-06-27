using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    [ApiController]
    public abstract class PricingStrategyControllerBase<TEntity, TResource, TCompiler>
        : PalavyrBaseController, IDynamicTableController<TEntity, TResource>
        where TEntity : class, IPricingStrategyTable<TEntity>, IEntity, new()
        where TResource : class, IPricingStrategyTableRowResource, new()
        where TCompiler : class, IPricingStrategyTableCompiler
    {
        private readonly IMediator mediator;
        public const string BaseRoute = "api/tables/dynamic/";
        public PricingStrategyControllerBase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpDelete("intent/{intentId}/table/{tableId}")]
        public async Task Delete([FromRoute] string intentId, [FromRoute] string tableId, CancellationToken cancellationToken)
        {
            await mediator.Send(
                new DeletePricingStrategyTableRequest<TEntity, TResource, TCompiler>()
                {
                    IntentId = intentId,
                    TableId = tableId
                }, cancellationToken);
        }

        [HttpPut("intent/{intentId}/table/{tableId}")]
        public async Task<IEnumerable<TResource>> SaveTable([FromRoute] string intentId, [FromRoute] string tableId, [FromBody] PricingStrategyTable<TResource> pricingStrategyTable)
        {
            var resource = await mediator.Send(
                new SavePricingStrategyTableRequest<TEntity, TResource, TCompiler>()
                {
                    TableId = tableId,
                    IntentId = intentId,
                    PricingStrategyTableResource = pricingStrategyTable
                });
            return resource.Resource;
        }

        [HttpGet("intent/{intentId}/table/{tableId}")]
        public async Task<PricingStrategyTableDataResource<TResource>> GetTable([FromRoute] string intentId, [FromRoute] string tableId)
        {
            var response = await mediator.Send(
                new GetPricingStrategyTableRowsRequest<TEntity, TResource, TCompiler>()
                {
                    IntentId = intentId,
                    TableId = tableId
                });
            return response.Resource;
        }

        [HttpGet("intent/{intentId}/table/{tableId}/template")]
        public async Task<TResource> GetRowTemplate([FromRoute] string intentId, [FromRoute] string tableId)
        {
            var response = await mediator.Send(
                new GetPricingStrategyTableRowTemplateRequest<TEntity, TResource, TCompiler>()
                {
                    IntentId = intentId,
                    TableId = tableId
                });
            return response.Resource;
        }
        
        [HttpPost("tables/dynamic/{intentId}")]
        public async Task<PricingStrategyTableMetaResource> Create(
            [FromRoute]
            string intentId,
            CancellationToken cancellationToken)
        {
            // This should be part of the pricing Strategy
            var response = await mediator.Send(new CreatePricingStrategyTableRequest<TEntity, TResource, TCompiler>(intentId), cancellationToken);
            return response.Response;
        }
        
    }
}