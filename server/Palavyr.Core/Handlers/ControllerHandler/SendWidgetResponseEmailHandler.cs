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
        private readonly IMapToPreExisting<EmailRequest, ConversationRecord> mapper;
        private readonly IResponseEmailSender responseEmailSender;

        public SendWidgetResponseEmailHandler(
            IEntityStore<ConversationRecord> convoRecordStore,
            IMapToPreExisting<EmailRequest, ConversationRecord> mapper,
            IResponseEmailSender responseEmailSender)
        {
            this.convoRecordStore = convoRecordStore;
            this.mapper = mapper;
            this.responseEmailSender = responseEmailSender;
        }

        public async Task<SendWidgetResponseEmailResponse> Handle(SendWidgetResponseEmailRequest request, CancellationToken cancellationToken)
        {
            if (!request.IsDemo)
            {
                var convoRecord = await convoRecordStore.Get(request.EmailRequest.ConversationId, s => s.ConversationId);
                await mapper.Map(request.EmailRequest, convoRecord, cancellationToken);
                await convoRecordStore.Update(convoRecord);
            }

            var resultResponse = await responseEmailSender.SendWidgetResponse(request.IntentId, request.EmailRequest, request.IsDemo);
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
        public SendWidgetResponseEmailRequest(EmailRequest emailRequest, string intentId, bool isDemo)
        {
            EmailRequest = emailRequest;
            IntentId = intentId;
            IsDemo = isDemo;
        }
        

        public EmailRequest EmailRequest { get; set; }
        public string IntentId { get; set; }
        public bool IsDemo { get; set; }
    }
}