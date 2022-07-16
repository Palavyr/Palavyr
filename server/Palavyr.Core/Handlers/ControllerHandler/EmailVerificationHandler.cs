using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class EmailVerificationHandler : IRequestHandler<EmailAddressVerificationRequest, EmailAddressVerificationResponse>
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly IEmailVerificationStatus emailVerificationStatus;
        private readonly ILogger<EmailVerificationHandler> logger;

        public EmailVerificationHandler(
            IEntityStore<Intent> intentStore,
            IEmailVerificationStatus emailVerificationStatus,
            ILogger<EmailVerificationHandler> logger
        )
        {
            this.intentStore = intentStore;
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
        }

        public async Task<EmailAddressVerificationResponse> Handle(EmailAddressVerificationRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            var verificationResponse = await emailVerificationStatus.GetVerificationResponse(request.EmailAddress);

            area.EmailIsVerified = verificationResponse.IsVerified();
            area.AwaitingVerification = verificationResponse.IsPending();

            if (!verificationResponse.IsFailed())
            {
                area.AreaSpecificEmail = request.EmailAddress;
            }

            return new EmailAddressVerificationResponse(verificationResponse);
        }
    }

    public class EmailAddressVerificationResponse
    {
        public EmailAddressVerificationResponse(EmailVerificationResponse response) => Response = response;
        public EmailVerificationResponse Response { get; set; }
    }

    public class EmailAddressVerificationRequest : IRequest<EmailAddressVerificationResponse>
    {
        public string EmailAddress { get; set; }
        public string IntentId { get; set; }
    }
}