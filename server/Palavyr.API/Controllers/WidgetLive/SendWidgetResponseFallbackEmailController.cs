using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.EmailService.EmailResponse;

namespace Palavyr.API.Controllers.WidgetLive
{
    public class SendWidgetResponseFallbackEmailController : PalavyrBaseController
    {
        private readonly IResponseEmailSender responseEmailSender;

        public SendWidgetResponseFallbackEmailController(
            IResponseEmailSender responseEmailSender
        )
        {
            this.responseEmailSender = responseEmailSender;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpPost("widget/area/{areaId}/email/fallback/send")]
        public async Task<SendEmailResultResponse> SendEmail(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromBody] EmailRequest emailRequest,
            CancellationToken cancellationToken
        )
        {
            var fallbackResultResponse = await responseEmailSender.SendFallbackEmail(
                accountId,
                areaId,
                emailRequest,
                cancellationToken
            );
            return fallbackResultResponse;
        }
    }
}