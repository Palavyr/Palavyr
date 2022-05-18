using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    public class SendWidgetResponseEmailController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget/area/{intentId}/email/send";

        public SendWidgetResponseEmailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpPost(Route)]
        public async Task<SendLiveEmailResultResource> SendEmail(
            [FromRoute]
            string intentId,
            [FromQuery] bool demo,
            [FromBody]
            EmailRequest emailRequest,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(new SendWidgetResponseEmailRequest(emailRequest, intentId, demo), cancellationToken);
            return response.Resource;
        }
    }
}