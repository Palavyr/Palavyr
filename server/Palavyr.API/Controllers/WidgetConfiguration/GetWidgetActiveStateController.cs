using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    public class GetWidgetActiveStateController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public GetWidgetActiveStateController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet("widget-config/widget-active-state")]
        public async Task<bool> GetWidgetActiveState(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetWidgetActiveStateRequest());
            return response.ActiveState;
        }
    }
}