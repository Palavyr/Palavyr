using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
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
        public async Task<SendEmailResultResponse> SendEmail(
            [FromRoute]
            string intentId,
            [FromBody]
            EmailRequest emailRequest,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(new SendWidgetResponseEmailRequest(emailRequest, intentId), cancellationToken);
            return response.Response;
        }
    }
}