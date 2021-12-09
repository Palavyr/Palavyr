using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.EmailService.EmailResponse;

namespace Palavyr.API.Controllers.WidgetLive
{
    public class SendWidgetResponseEmailHandler : ISendWidgetResponseEmailHandler
    {
        private readonly IConvoHistoryRepository convoRepository;
        private readonly IResponseEmailSender responseEmailSender;

        public SendWidgetResponseEmailHandler(
            IConvoHistoryRepository convoRepository,
            IResponseEmailSender responseEmailSender)
        {
            this.convoRepository = convoRepository;
            this.responseEmailSender = responseEmailSender;
        }

        public async Task<SendEmailResultResponse> HandleSendWidgetResponseEmail(EmailRequest emailRequest, string accountId, string areaId, CancellationToken cancellationToken)
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