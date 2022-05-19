using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.PricingStrategyHandlers
{
    public class SavePricingStrategyTableHandler<T, TR> 
        : IRequestHandler<SavePricingStrategyTableRequest<T, TR>, SavePricingStrategyTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, new()
        where TR : IPricingStrategyTableRowResource
    {
        private readonly IPricingStrategyTableCommandExecutor<T> executor;
        private readonly IMapToNew<T, TR> entityMapper;

        public SavePricingStrategyTableHandler(
            IPricingStrategyTableCommandExecutor<T> executor,
            IMapToNew<T, TR> entityMapper
        )
        {
            this.executor = executor;
            this.entityMapper = entityMapper;
        }

        public async Task<SavePricingStrategyTableResponse<TR>> Handle(SavePricingStrategyTableRequest<T, TR> request, CancellationToken cancellationToken)
        {
            var rows = await executor.SaveTable(request.IntentId, request.TableId, request.PricingStrategyTable);
            var resource = await entityMapper.MapMany(rows);
            return new SavePricingStrategyTableResponse<TR>(resource);
        }
    }

    public class SavePricingStrategyTableRequest<T, TR> : IRequest<SavePricingStrategyTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, new()
        where TR : IPricingStrategyTableRowResource
    {
        public string IntentId { get; set; }
        public string TableId { get; set; }
        public PricingStrategyTable<T> PricingStrategyTable { get; set; }
    }

    public class SavePricingStrategyTableResponse<TR> where TR : IPricingStrategyTableRowResource
    {
        public SavePricingStrategyTableResponse(IEnumerable<TR> resource) => Resource = resource;
        public IEnumerable<TR> Resource { get; set; }
    }
}