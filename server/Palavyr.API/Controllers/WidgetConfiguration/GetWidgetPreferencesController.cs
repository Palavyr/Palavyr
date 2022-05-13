using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    public class GetWidgetPreferencesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget-config/preferences";

        public GetWidgetPreferencesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<WidgetPreferenceResource> GetWidgetPreferences(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetWidgetPreferencesRequest(), cancellationToken);
            return response.Response;
        }
    }
}