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
    public class SendWidgetResponseFallbackEmailController : PalavyrBaseController
    {
        private readonly IMediator mediator;


        public SendWidgetResponseFallbackEmailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpPost(SendWidgetResponseFallbackEmailRequest.Route)]
        public async Task<SendLiveEmailResultResource> SendEmail(
            [FromQuery]
            bool demo,
            [FromRoute]
            string intentId,
            [FromBody]
            EmailRequest emailRequest,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(new SendWidgetResponseFallbackEmailRequest(emailRequest, intentId, demo), cancellationToken);
            return response.Resource;
        }
    }
}