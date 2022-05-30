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
    public class GetPricingStrategyTableRowTemplateHandler<T, TR, TCompiler>
        : IRequestHandler<GetPricingStrategyTableRowTemplateRequest<T, TR, TCompiler>, GetPricingStrategyTableRowTemplateResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, ITable, new()
        where TR : IPricingStrategyTableRowResource
        where TCompiler : IPricingStrategyTableCompiler
    {
        private readonly IPricingStrategyTableCommandExecutor<T, TCompiler> executor;
        private readonly IMapToNew<T, TR> entityMapper;

        public GetPricingStrategyTableRowTemplateHandler(
            IPricingStrategyTableCommandExecutor<T, TCompiler> executor,
            IMapToNew<T, TR> entityMapper
        )
        {
            this.executor = executor;
            this.entityMapper = entityMapper;
        }

        public async Task<GetPricingStrategyTableRowTemplateResponse<TR>> Handle(GetPricingStrategyTableRowTemplateRequest<T, TR, TCompiler> request, CancellationToken cancellationToken)
        {
            var template = executor.GetRowTemplate(request.IntentId, request.TableId);
            var resource = await entityMapper.Map(template, cancellationToken);
            return new GetPricingStrategyTableRowTemplateResponse<TR>(resource);
        }
    }

    public class GetPricingStrategyTableRowTemplateRequest<T, TR, TCompiler>
        : IRequest<GetPricingStrategyTableRowTemplateResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, new()
        where TR : IPricingStrategyTableRowResource
        where TCompiler : IPricingStrategyTableCompiler
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