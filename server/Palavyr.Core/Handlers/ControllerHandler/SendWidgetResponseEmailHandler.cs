using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources;
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
                if (request.EmailRequest.ConversationId is null) throw new DomainException("Conversation Id must be supplied in production");
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
        public SendWidgetResponseEmailResponse(SendLiveEmailResultResource resource) => Resource = resource;
        public SendLiveEmailResultResource Resource { get; set; }
    }

    public class SendWidgetResponseEmailRequest : IRequest<SendWidgetResponseEmailResponse>
    {
        public const string Route = "widget/area/{intentId}/email/send";

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