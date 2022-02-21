using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class GetLiveWidgetActiveStateController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget/widget-active-state";

        public GetLiveWidgetActiveStateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<bool> GetWidgetActiveState(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetLiveWidgetActiveStateRequest(), cancellationToken);
            return response.Response;
        }
    }
}