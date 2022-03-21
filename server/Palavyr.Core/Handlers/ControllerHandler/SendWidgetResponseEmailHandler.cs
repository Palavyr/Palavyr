using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.EmailService.EmailResponse;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SendWidgetResponseEmailHandler : IRequestHandler<SendWidgetResponseEmailRequest, SendWidgetResponseEmailResponse>
    {
        private readonly IEntityStore<ConversationRecord> convoRecordStore;
        private readonly IMapToNew<EmailRequest, ConversationRecord> mapper;
        private readonly IResponseEmailSender responseEmailSender;

        public SendWidgetResponseEmailHandler(
            IEntityStore<ConversationRecord> convoRecordStore,
            IMapToNew<EmailRequest, ConversationRecord> mapper,
            IResponseEmailSender responseEmailSender)
        {
            this.convoRecordStore = convoRecordStore;
            this.mapper = mapper;
            this.responseEmailSender = responseEmailSender;
        }

        public async Task<SendWidgetResponseEmailResponse> Handle(SendWidgetResponseEmailRequest request, CancellationToken cancellationToken)
        {
            var convoRecord = await convoRecordStore.Get(request.EmailRequest.ConversationId, s => s.ConversationId);
            var updatedRecord = await mapper.Map(request.EmailRequest);
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