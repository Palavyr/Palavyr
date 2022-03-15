using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class GetWidgetPreCheckController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget/pre-check";


        public GetWidgetPreCheckController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<PreCheckResult> Get([FromQuery] bool demo, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetWidgetPreCheckRequest(), cancellationToken);
            return response.Response;
        }
    }
}