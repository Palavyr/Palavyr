using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.EmailService.EmailResponse;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SendWidgetResponseEmailHandler : IRequestHandler<SendWidgetResponseEmailRequest, SendWidgetResponseEmailResponse>
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

        public async Task<SendWidgetResponseEmailResponse> Handle(SendWidgetResponseEmailRequest request, CancellationToken cancellationToken)
        {
            var convoRecord = await convoRepository.GetConversationRecordById(request.EmailRequest.ConversationId);
            var updatedRecord = convoRecord.ApplyEmailRequest(request.EmailRequest);
            await convoRepository.UpdateConversationRecord(updatedRecord);

            var resultResponse = await responseEmailSender.SendEmail(request.IntentId, request.EmailRequest);
            await convoRepository.CommitChangesAsync();
            return new SendWidgetResponseEmailResponse(resultResponse);
        }
    }

    public class SendWidgetResponseEmailResponse
    {
        public SendWidgetResponseEmailResponse(SendEmailResultResponse response) => Response = response;
        public SendEmailResultResponse Response { get; set; }
    }

    public class SendWidgetResponseEmailRequest : IRequest<SendWidgetResponseEmailResponse>
    {
        public SendWidgetResponseEmailRequest(EmailRequest emailRequest, string intentId)
        {
            EmailRequest = emailRequest;
            IntentId = intentId;
        }

        public EmailRequest EmailRequest { get; set; }
        public string IntentId { get; set; }
    }
}