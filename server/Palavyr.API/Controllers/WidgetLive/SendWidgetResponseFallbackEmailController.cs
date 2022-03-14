using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.EmailService.EmailResponse;

namespace Palavyr.API.Controllers.WidgetLive
{
    public class SendWidgetResponseFallbackEmailController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "widget/area/{areaId}/email/fallback/send";

        public SendWidgetResponseFallbackEmailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpPost(Route)]
        public async Task<SendEmailResultResponse> SendEmail(
            [FromRoute]
            string areaId,
            [FromBody]
            EmailRequest emailRequest,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(new SendWidgetResponseFallbackEmailRequest(emailRequest, areaId), cancellationToken);
            return response.Response;
        }
    }
}