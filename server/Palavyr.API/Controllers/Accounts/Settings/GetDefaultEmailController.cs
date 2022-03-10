using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetDefaultEmailController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/email";

        public GetDefaultEmailController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<AccountEmailSettingsResponse> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetDefaultEmailRequest(), cancellationToken);
            return response.Response;
        }
    }
}