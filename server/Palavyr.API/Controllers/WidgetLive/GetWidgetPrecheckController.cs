using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class GetWidgetPreCheckController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget/pre-check";

        private readonly IConfigurationRepository configurationRepository;
        private readonly IWidgetStatusChecker widgetStatusChecker;
        private ILogger<GetWidgetPreCheckController> logger;
        private readonly IAccountIdTransport accountIdTransport;

        public GetWidgetPreCheckController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<PreCheckResult> Get([FromQuery] bool demo, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetWidgetPrecheckRequest(), cancellationToken);
            return response.Response;
        }
    }
}