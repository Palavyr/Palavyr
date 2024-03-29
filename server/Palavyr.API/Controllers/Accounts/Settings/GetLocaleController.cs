using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

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
        public async Task<LocaleMetaResource> Get([FromQuery] bool read, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetCurrentLocalAndLocaleMapRequest() { Read = read }, cancellationToken);
            return response.Resource;
        }
    }
}