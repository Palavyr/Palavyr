using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetTableNameMapHandler : IRequestHandler<GetTableNameMapRequest, GetTableNameMapResponse>
    {
        private readonly IMapToNew<PricingStrategyType, PricingStrategyTableTypeResource> mapper;

        public GetTableNameMapHandler(IMapToNew<PricingStrategyType, PricingStrategyTableTypeResource> mapper)
        {
            this.mapper = mapper;
        }

        public async Task<GetTableNameMapResponse> Handle(GetTableNameMapRequest request, CancellationToken cancellationToken)
        {
            // map that provides e.g. Select One Flat: SelectOneFlat.
            var availableTables = PricingStrategyTableTypes.GetDynamicTableTypes();
            var resource = await mapper.MapMany(availableTables);
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