using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    public class ModifyWidgetActiveStateController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget-config/widget-active-state";


        public ModifyWidgetActiveStateController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<bool> ModifyWidgetActiveState(
            [FromQuery]
            bool state,
            CancellationToken cancellationToken
        )
        {
            if (state == null)
            {
                state = true;
            }

            var response = await mediator.Send(new ModifyWidgetActiveStateRequest(state), cancellationToken);
            return response.Response;
        }
    }
}