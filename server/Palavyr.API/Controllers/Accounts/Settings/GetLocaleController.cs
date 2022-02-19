using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Services.Localization;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetLocaleController : PalavyrBaseController
    {
        public const string Route = "account/settings/locale";
        private readonly IMediator mediator;

        public GetLocaleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<CurrentLocaleAndLocalMapRetriever.LocaleResponse> Get([FromQuery] bool read, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetCurrentLocalAndLocaleMapRequest(){ Read = read}, cancellationToken);
            return response.Response;
        }
    }
}