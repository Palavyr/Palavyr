using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.PricingStrategyHandlers
{
    public class GetPricingStrategyTableRowsHandler<T, TR, TCompiler> 
        : IRequestHandler<GetPricingStrategyTableRowsRequest<T, TR, TCompiler>, GetPricingStrategyTableRowsResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, ITable, new()
        where TR : class, IPricingStrategyTableRowResource
        where TCompiler : class, IPricingStrategyTableCompiler
    {
        private readonly IPricingStrategyTableCommandExecutor<T, TR, TCompiler> executor;
        private readonly IMapToNew<T, TR> entityMapper;

        public GetPricingStrategyTableRowsHandler(
            IPricingStrategyTableCommandExecutor<T, TR, TCompiler> executor,
            IMapToNew<T, TR> entityMapper
        )
        {
            this.executor = executor;
            this.entityMapper = entityMapper;
        }

        public async Task<GetPricingStrategyTableRowsResponse<TR>> Handle(GetPricingStrategyTableRowsRequest<T, TR, TCompiler> request, CancellationToken cancellationToken)
        {
            var data = await executor.GetTableRows(request.IntentId, request.TableId);
            var mapped = await entityMapper.MapMany(data.TableRows, cancellationToken);
            var resource = new PricingStrategyTableDataResource<TR>()
            {
                TableRows = mapped,
                IsInUse = data.IsInUse
            };
            return new GetPricingStrategyTableRowsResponse<TR>(resource);
        }
    }

    public class GetPricingStrategyTableRowsRequest<T, TR, TCompiler> : IRequest<GetPricingStrategyTableRowsResponse<TR>> 
        where TR : IPricingStrategyTableRowResource
        where TCompiler : IPricingStrategyTableCompiler
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