using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.PricingStrategyHandlers
{
    public class GetPricingStrategyTableRowTemplateHandler<T, TR>
        : IRequestHandler<GetPricingStrategyTableRowTemplateRequest<T, TR>, GetPricingStrategyTableRowTemplateResponse<TR>>
        where T : class, IPricingStrategyTable<T>, new()
        where TR : IPricingStrategyTableRowResource
    {
        private readonly IPricingStrategyTableCommandExecutor<T> executor;
        private readonly IMapToNew<T, TR> entityMapper;

        public GetPricingStrategyTableRowTemplateHandler(
            IPricingStrategyTableCommandExecutor<T> executor,
            IMapToNew<T, TR> entityMapper
        )
        {
            this.executor = executor;
            this.entityMapper = entityMapper;
        }

        public async Task<GetPricingStrategyTableRowTemplateResponse<TR>> Handle(GetPricingStrategyTableRowTemplateRequest<T, TR> request, CancellationToken cancellationToken)
        {
            var template = executor.GetRowTemplate(request.IntentId, request.TableId);
            var resource = await entityMapper.Map(template);
            return new GetPricingStrategyTableRowTemplateResponse<TR>(resource);
        }
    }

    public class GetPricingStrategyTableRowTemplateRequest<T, TR>
        : IRequest<GetPricingStrategyTableRowTemplateResponse<TR>>
        where T : class, IPricingStrategyTable<T>, new()
        where TR : IPricingStrategyTableRowResource
    {
        public string IntentId { get; set; }
        public string TableId { get; set; }
    }

    public class GetPricingStrategyTableRowTemplateResponse<TR> where TR : IPricingStrategyTableRowResource
    {
        public GetPricingStrategyTableRowTemplateResponse(TR resource) => Resource = resource;
        public TR Resource { get; set; }
    }
}