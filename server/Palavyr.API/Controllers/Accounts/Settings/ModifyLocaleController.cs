using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class ModifyLocaleController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/locale";

        public ModifyLocaleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<LocaleResource> Modify(
            [FromBody]
            ModifyLocaleRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}