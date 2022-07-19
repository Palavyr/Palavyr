using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Response.Tables.PricingStrategy.Meta
{
    public class ModifyPricingStrategyTableMetaController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        

        public ModifyPricingStrategyTableMetaController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(ModifyPricingStrategyTableMetaRequest.Route)]
        public async Task<PricingStrategyTableMetaResource> Modify([FromBody] PricingStrategyTableMetaResource resource, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ModifyPricingStrategyTableMetaRequest(resource), cancellationToken);
            return response.Response;
        }
    }
}