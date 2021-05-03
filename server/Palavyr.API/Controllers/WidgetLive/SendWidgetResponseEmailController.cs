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

    public class SendWidgetResponseEmailController : PalavyrBaseController
    {
        private readonly IResponseEmailSender responseEmailSender;
        
        public SendWidgetResponseEmailController(
            IResponseEmailSender responseEmailSender
        )
        {
            this.responseEmailSender = responseEmailSender;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpPost("widget/area/{areaId}/email/send")]
        public async Task<SendEmailResultResponse> SendEmail(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromBody] EmailRequest emailRequest,
            CancellationToken cancellationToken
        )
        {
            var resultResponse = await responseEmailSender.SendEmail(
                accountId,
                areaId,
                emailRequest,
                cancellationToken
            );
            return resultResponse;
        }
    }
}