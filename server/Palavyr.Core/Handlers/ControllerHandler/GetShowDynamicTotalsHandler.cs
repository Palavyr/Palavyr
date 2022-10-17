using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetShowPricingStrategyTotalsHandlerHandler : IRequestHandler<GetShowPricingStrategyTotalsRequest, GetShowPricingStrategyTotalsHandlerResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public GetShowPricingStrategyTotalsHandlerHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetShowPricingStrategyTotalsHandlerResponse> Handle(GetShowPricingStrategyTotalsRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(request.IntentId, s => s.IntentId);
            return new GetShowPricingStrategyTotalsHandlerResponse(intent.IncludePricingStrategyTableTotals);
        }
    }

    public class GetShowPricingStrategyTotalsHandlerResponse
    {
        public GetShowPricingStrategyTotalsHandlerResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetShowPricingStrategyTotalsRequest : IRequest<GetShowPricingStrategyTotalsHandlerResponse>
    {
        public const string Route = "intent/pricing-strategy-totals/{intentId}";
        public string IntentId { get; set; }

        public GetShowPricingStrategyTotalsRequest(string intentId)
        {
            IntentId = intentId;
        }
    }
}