using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    public class GetDemoWidgetPreCheckController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget-config/demo/pre-check";


        public GetDemoWidgetPreCheckController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<PreCheckResultResource> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetDemoWidgetPreCheckRequest(), cancellationToken);
            return response.Response;
        }
    }
}