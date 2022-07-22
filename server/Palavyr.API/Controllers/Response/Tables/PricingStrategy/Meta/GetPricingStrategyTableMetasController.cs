using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Response.Tables.PricingStrategy.Meta
{
    public class GetPricingStrategyTableMetasController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "tables/pricing-strategy/metas/{intentId}";


        public GetPricingStrategyTableMetasController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<IEnumerable<PricingStrategyTableMetaResource>> Get(string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetPricingStrategyTableMetasRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}