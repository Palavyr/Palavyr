using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetTableNameMapHandler : IRequestHandler<GetTableNameMapRequest, GetTableNameMapResponse>
    {
        private readonly IMapToNew<IHaveAPrettyNameAndTableType, PricingStrategyTableTypeResource> mapper;
        private readonly PricingStrategyTypeLister pricingStrategyTypeLister;

        public GetTableNameMapHandler(IMapToNew<IHaveAPrettyNameAndTableType, PricingStrategyTableTypeResource> mapper, PricingStrategyTypeLister pricingStrategyTypeLister)
        {
            this.mapper = mapper;
            this.pricingStrategyTypeLister = pricingStrategyTypeLister;
        }

        public async Task<GetTableNameMapResponse> Handle(GetTableNameMapRequest request, CancellationToken cancellationToken)
        {
            // map that provides e.g. Select One Flat: SelectOneFlat.
            var availableTables = pricingStrategyTypeLister.ListPricingStrategies();
            var resource = await mapper.MapMany(availableTables, cancellationToken);
            return new GetTableNameMapResponse(resource);
        }
    }

    public class GetTableNameMapResponse
    {
        public GetTableNameMapResponse(IEnumerable<PricingStrategyTableTypeResource> response) => Response = response;
        public IEnumerable<PricingStrategyTableTypeResource> Response { get; set; }
    }

    public class GetTableNameMapRequest : IRequest<GetTableNameMapResponse>
    {
    }
}