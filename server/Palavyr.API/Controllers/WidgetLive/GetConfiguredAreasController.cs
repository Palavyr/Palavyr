using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class GetConfiguredAreasController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget/intents";


        public GetConfiguredAreasController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<List<Area>> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetConfiguredIntentsRequest(), cancellationToken);
            return response.Response;
        }
    }
}