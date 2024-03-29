using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    public class ModifyWidgetPreferencesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget-config/preferences";

        public ModifyWidgetPreferencesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<WidgetPreferencesResource> SaveWidgetPreferences(
            [FromBody]
            ModifyWidgetPreferencesRequest request,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}