using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyShowPricingStrategyTableTotalsHandler : IRequestHandler<ModifyShowPricingStrategyTableTotalsRequest, ModifyShowPricingStrategyTotalsResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public ModifyShowPricingStrategyTableTotalsHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyShowPricingStrategyTotalsResponse> Handle(ModifyShowPricingStrategyTableTotalsRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(request.IntentId, s => s.IntentId);
            intent.IncludePricingStrategyTableTotals = request.ShowPricingStrategyTotals;
            return new ModifyShowPricingStrategyTotalsResponse(intent.IncludePricingStrategyTableTotals);
        }
    }

    public class ModifyShowPricingStrategyTotalsResponse
    {
        public ModifyShowPricingStrategyTotalsResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ModifyShowPricingStrategyTableTotalsRequest : IRequest<ModifyShowPricingStrategyTotalsResponse>
    {
        public string IntentId { get; set; }
        public bool ShowPricingStrategyTotals { get; set; }
    }
}