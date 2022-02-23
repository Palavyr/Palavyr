using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.Localization;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetLocaleForWidgetController : PalavyrBaseController
    {
        public const string Route = "account/settings/locale/widget";
        private readonly IMediator mediator;

        public GetLocaleForWidgetController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpGet(Route)]
        public async Task<CurrentLocaleAndLocalMapRetriever.LocaleResponse> GetForWidget(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetCurrentLocalAndLocaleMapRequest(), cancellationToken);
            return response.Response;
        }
    }
}