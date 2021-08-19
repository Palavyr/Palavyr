using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.EmailService.EmailResponse;

namespace Palavyr.API.Controllers.WidgetLive
{
    public class SendWidgetResponseEmailController : PalavyrBaseController
    {
        private readonly IConvoHistoryRepository convoRepository;
        private readonly IResponseEmailSender responseEmailSender;

        public SendWidgetResponseEmailController(
            IConvoHistoryRepository convoRepository,
            IResponseEmailSender responseEmailSender
        )
        {
            this.convoRepository = convoRepository;
            this.responseEmailSender = responseEmailSender;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpPost("widget/area/{areaId}/email/send")]
        public async Task<SendEmailResultResponse> SendEmail(
            [FromHeader]
            string accountId,
            [FromRoute]
            string areaId,
            [FromBody]
            EmailRequest emailRequest,
            CancellationToken cancellationToken
        )
        {
            var convoRecord = await convoRepository.GetConversationRecordById(emailRequest.ConversationId);
            var updatedRecord = convoRecord.ApplyEmailRequest(emailRequest);
            await convoRepository.UpdateConversationRecord(updatedRecord);

            var resultResponse = await responseEmailSender.SendEmail(
                accountId,
                areaId,
                emailRequest,
                cancellationToken
            );
            await convoRepository.CommitChangesAsync();
            return resultResponse;
        }
    }
}