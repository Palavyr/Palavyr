using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.EmailService.EmailResponse;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SendWidgetResponseFallbackEmailHandler : IRequestHandler<SendWidgetResponseFallbackEmailRequest, SendWidgetResponseFallbackEmailResponse>
    {
        private readonly IResponseEmailSender responseEmailSender;

        public SendWidgetResponseFallbackEmailHandler(IResponseEmailSender responseEmailSender)
        {
            this.responseEmailSender = responseEmailSender;
        }

        public async Task<SendWidgetResponseFallbackEmailResponse> Handle(SendWidgetResponseFallbackEmailRequest request, CancellationToken cancellationToken)
        {
            var sendLiveEmailResultResource = await responseEmailSender.SendFallbackResponse(request.IntentId, request.EmailRequest, request.IsDemo);
            return new SendWidgetResponseFallbackEmailResponse(sendLiveEmailResultResource);
        }
    }

    public class SendWidgetResponseFallbackEmailResponse
    {
        public SendWidgetResponseFallbackEmailResponse(SendLiveEmailResultResource resource) => Resource = resource;
        public SendLiveEmailResultResource Resource { get; set; }
    }

    public class SendWidgetResponseFallbackEmailRequest : IRequest<SendWidgetResponseFallbackEmailResponse>
    {

        public const string Route = "widget/intent/{intentId}/email/fallback/send";
        
        public SendWidgetResponseFallbackEmailRequest(EmailRequest emailRequest, string intentId, bool isDemo)
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