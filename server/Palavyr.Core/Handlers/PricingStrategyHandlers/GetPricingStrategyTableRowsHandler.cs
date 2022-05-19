using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.PricingStrategyHandlers
{
    // register these
    public class GetPricingStrategyTableRowsHandler<T, TR> : IRequestHandler<GetPricingStrategyTableRowsRequest<T, TR>, GetPricingStrategyTableRowsResponse<TR>>
        where T : class, IPricingStrategyTable<T>, new()
        where TR : IPricingStrategyTableRowResource
    {
        private readonly IPricingStrategyTableCommandExecutor<T> executor;
        private readonly IMapToNew<T, TR> entityMapper;

        public GetPricingStrategyTableRowsHandler(
            IPricingStrategyTableCommandExecutor<T> executor,
            IMapToNew<T, TR> entityMapper
        )
        {
            this.executor = executor;
            this.entityMapper = entityMapper;
        }

        public async Task<GetPricingStrategyTableRowsResponse<TR>> Handle(GetPricingStrategyTableRowsRequest<T, TR> request, CancellationToken cancellationToken)
        {
            var data = await executor.GetTableRows(request.IntentId, request.TableId);
            var mapped = await entityMapper.MapMany(data.TableRows);
            var resource = new PricingStrategyTableDataResource<TR>()
            {
                TableRows = mapped,
                IsInUse = data.IsInUse
            };
            return new GetPricingStrategyTableRowsResponse<TR>(resource);
        }
    }

    public class GetPricingStrategyTableRowsRequest<T, TR> : IRequest<GetPricingStrategyTableRowsResponse<TR>> 
        where TR : IPricingStrategyTableRowResource
    {
        public string IntentId { get; set; }
        public string TableId { get; set; }
    }

    public class GetPricingStrategyTableRowsResponse<TR> where TR : IPricingStrategyTableRowResource
    {
        public GetPricingStrategyTableRowsResponse(PricingStrategyTableDataResource<TR> resource) => Resource = resource;
        public PricingStrategyTableDataResource<TR> Resource { get; set; }
    }
}