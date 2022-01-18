using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;

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
        public async Task<WidgetPreference> GetWidgetPreferences(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetWidgetPreferencesRequest(), cancellationToken);
            return response.Response;
        }
    }
}