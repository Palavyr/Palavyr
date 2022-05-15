using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class GetLiveWidgetPreferencesController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "widget/preferences";

        public GetLiveWidgetPreferencesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<WidgetPreferenceResource> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetWidgetPreferencesRequest(), cancellationToken);
            return response.Response;
        }
    }
}