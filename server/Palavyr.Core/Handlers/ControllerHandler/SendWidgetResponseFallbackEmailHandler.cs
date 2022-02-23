using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
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
            var fallbackResultResponse = await responseEmailSender.SendFallbackEmail(
                request.IntentId,
                request.EmailRequest,
                cancellationToken
            );
            return new SendWidgetResponseFallbackEmailResponse(fallbackResultResponse);
        }
    }

    public class SendWidgetResponseFallbackEmailResponse
    {
        public SendWidgetResponseFallbackEmailResponse(SendEmailResultResponse response) => Response = response;
        public SendEmailResultResponse Response { get; set; }
    }

    public class SendWidgetResponseFallbackEmailRequest : IRequest<SendWidgetResponseFallbackEmailResponse>
    {
        public SendWidgetResponseFallbackEmailRequest(EmailRequest emailRequest, string intentId)
        {
            EmailRequest = emailRequest;
            IntentId = intentId;
        }

        public EmailRequest EmailRequest { get; set; }
        public string IntentId { get; set; }
    }
}