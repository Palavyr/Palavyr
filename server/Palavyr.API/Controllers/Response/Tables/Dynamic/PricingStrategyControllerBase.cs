using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Common.ExtensionMethods.PathExtensions;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
using Palavyr.Core.Models.Contracts;
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

        [HttpDelete(DeletePricingStrategyTableRequest<TEntity, TResource, TCompiler>.Route)]
        public async Task Delete([FromRoute] string intentId, [FromRoute] string tableId, CancellationToken cancellationToken)
        {
            await mediator.Send(
                new DeletePricingStrategyTableRequest<TEntity, TResource, TCompiler>()
                {
                    IntentId = intentId,
                    TableId = tableId
                }, cancellationToken);
        }

        [HttpPut(SavePricingStrategyTableRequest<TEntity, TResource, TCompiler>.Route)]
        public async Task<PricingStrategyTableDataResource<TResource>> SaveTable([FromRoute] string intentId, [FromRoute] string tableId, [FromBody] PricingStrategyTableDataResource<TResource> pricingStrategyTable, CancellationToken cancellationToken)
        {
            var resource = await mediator.Send(
                new SavePricingStrategyTableRequest<TEntity, TResource, TCompiler>()
                {
                    TableId = tableId,
                    IntentId = intentId,
                    PricingStrategyTableResource = pricingStrategyTable
                }, cancellationToken);
            return resource.Resource;
        }

        [HttpGet(GetPricingStrategyTableRowsRequest<TResource, TResource, TCompiler>.Route)]
        public async Task<PricingStrategyTableDataResource<TResource>> GetTable([FromRoute] string intentId, [FromRoute] string tableId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(
                new GetPricingStrategyTableRowsRequest<TEntity, TResource, TCompiler>()
                {
                    IntentId = intentId,
                    TableId = tableId
                }, cancellationToken);
            return response.Resource;
        }


        [HttpGet(GetPricingStrategyTableRowTemplateRequest<TEntity, TResource, TCompiler>.Route)]
        public async Task<TResource> GetRowTemplate([FromRoute] string intentId, [FromRoute] string tableId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(
                new GetPricingStrategyTableRowTemplateRequest<TEntity, TResource, TCompiler>()
                {
                    IntentId = intentId,
                    TableId = tableId
                }, cancellationToken);
            return response.Resource;
        }

        [HttpPost(CreatePricingStrategyTableRequest<TEntity, TResource, TCompiler>.Route)]
        public async Task<PricingStrategyTableMetaResource> Create(
            [FromRoute]
            string intentId,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new CreatePricingStrategyTableRequest<TEntity, TResource, TCompiler>(intentId), cancellationToken);
            return response.Response;
        }

        public static string AssembleRoute<T>(string trunk)
        {
            var getRoute = UriUtils.Join(
                BaseRoute,
                typeof(T).Name,
                trunk
            ).Replace("api/", "");
            return getRoute;
        }
    }
}