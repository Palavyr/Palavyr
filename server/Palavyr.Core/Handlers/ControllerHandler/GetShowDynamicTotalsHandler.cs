using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetShowPricingStrategyTotalsHandlerHandler : IRequestHandler<GetShowPricingStrategyTotalsHandlerRequest, GetShowPricingStrategyTotalsHandlerResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public GetShowPricingStrategyTotalsHandlerHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetShowPricingStrategyTotalsHandlerResponse> Handle(GetShowPricingStrategyTotalsHandlerRequest request, CancellationToken cancellationToken)
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

    public class GetShowPricingStrategyTotalsHandlerRequest : IRequest<GetShowPricingStrategyTotalsHandlerResponse>
    {
        public string IntentId { get; set; }

        public GetShowPricingStrategyTotalsHandlerRequest(string intentId)
        {
            IntentId = intentId;
        }
    }
}