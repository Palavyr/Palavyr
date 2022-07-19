using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Intents
{
    public class GetShowPricingStrategyTableTotals : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "intent/pricing-strategy-totals/{intentId}";

        public GetShowPricingStrategyTableTotals(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<bool> Get(
            [FromRoute]
            string intentId,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetShowPricingStrategyTotalsHandlerRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}