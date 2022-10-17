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

        public GetShowPricingStrategyTableTotals(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(GetShowPricingStrategyTotalsRequest.Route)]
        public async Task<bool> Get(
            [FromRoute] string intentId,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetShowPricingStrategyTotalsRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}