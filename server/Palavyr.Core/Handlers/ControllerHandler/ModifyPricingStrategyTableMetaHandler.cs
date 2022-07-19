using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.Units;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyPricingStrategyTableMetaHandler : IRequestHandler<ModifyPricingStrategyTableMetaRequest, ModifyPricingStrategyTableMetaResponse>
    {
        private readonly IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore;
        private readonly IMapToPreExisting<PricingStrategyTableMetaResource, PricingStrategyTableMeta> mapper;
        private readonly IMapToNew<PricingStrategyTableMeta, PricingStrategyTableMetaResource> resourceMapper;
        private readonly IUnitRetriever unitRetriever;

        public ModifyPricingStrategyTableMetaHandler(
            IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore,
            IMapToPreExisting<PricingStrategyTableMetaResource, PricingStrategyTableMeta> mapper,
            IMapToNew<PricingStrategyTableMeta, PricingStrategyTableMetaResource> resourceMapper,
            IUnitRetriever unitRetriever)
        {
            this.pricingStrategyTableMetaStore = pricingStrategyTableMetaStore;
            this.mapper = mapper;
            this.resourceMapper = resourceMapper;
            this.unitRetriever = unitRetriever;
        }

        public async Task<ModifyPricingStrategyTableMetaResponse> Handle(ModifyPricingStrategyTableMetaRequest request, CancellationToken cancellationToken)
        {
            var currentMeta = await pricingStrategyTableMetaStore.Get(request.Resource.TableId, s => s.TableId);

            await mapper.Map(request.Resource, currentMeta, cancellationToken);

            var updatedMeta = await pricingStrategyTableMetaStore.Update(currentMeta);

            var resource = await resourceMapper.Map(updatedMeta, cancellationToken);

            return new ModifyPricingStrategyTableMetaResponse(resource);
        }
    }

    public class ModifyPricingStrategyTableMetaResponse
    {
        public ModifyPricingStrategyTableMetaResponse(PricingStrategyTableMetaResource response) => Response = response;
        public PricingStrategyTableMetaResource Response { get; set; }
    }

    public class ModifyPricingStrategyTableMetaRequest : IRequest<ModifyPricingStrategyTableMetaResponse>
    {
        public const string Route = "tables/pricing-strategy/modify";

        public PricingStrategyTableMetaResource Resource { get; }

        public ModifyPricingStrategyTableMetaRequest(PricingStrategyTableMetaResource resource)
        {
            Resource = resource;
        }
    }
}