using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Intents
{
    [Obsolete]
    public class ModifyShowPricingStrategyTableTotalsController : PalavyrBaseController
    {
        public const string Route = "intent/pricing-strategy-totals";
        private readonly IMediator mediator;

        public ModifyShowPricingStrategyTableTotalsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<bool> Post(
            ModifyShowPricingStrategyTableTotalsRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}