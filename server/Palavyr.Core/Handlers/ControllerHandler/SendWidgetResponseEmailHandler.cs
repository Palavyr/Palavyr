using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.EmailService.EmailResponse;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SendWidgetResponseEmailHandler : IRequestHandler<SendWidgetResponseEmailRequest, SendWidgetResponseEmailResponse>
    {
        private readonly IEntityStore<ConversationRecord> convoRecordStore;
        private readonly IResponseEmailSender responseEmailSender;

        public SendWidgetResponseEmailHandler(
            IEntityStore<ConversationRecord> convoRecordStore,
            IResponseEmailSender responseEmailSender)
        {
            this.convoRecordStore = convoRecordStore;
            this.responseEmailSender = responseEmailSender;
        }

        public async Task<SendWidgetResponseEmailResponse> Handle(SendWidgetResponseEmailRequest request, CancellationToken cancellationToken)
        {
            var convoRecord = await convoRecordStore.Get(request.EmailRequest.ConversationId, s => s.ConversationId);
            var updatedRecord = convoRecord.ApplyEmailRequest(request.EmailRequest);
            await convoRecordStore.Update(updatedRecord);

            var resultResponse = await responseEmailSender.SendEmail(request.IntentId, request.EmailRequest);
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