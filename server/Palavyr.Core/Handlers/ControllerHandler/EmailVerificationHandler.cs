using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.EmailService.Verification;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class EmailVerificationHandler : IRequestHandler<EmailAddressVerificationRequest, EmailAddressVerificationResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IEmailVerificationStatus emailVerificationStatus;
        private readonly ILogger<EmailVerificationHandler> logger;

        public EmailVerificationHandler(
            IConfigurationRepository configurationRepository,
            IEmailVerificationStatus emailVerificationStatus,
            ILogger<EmailVerificationHandler> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
        }

        public async Task<EmailAddressVerificationResponse> Handle(EmailAddressVerificationRequest request, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(request.IntentId);
            var verificationResponse = await emailVerificationStatus.GetVerificationResponse(request.EmailAddress);

            area.EmailIsVerified = verificationResponse.IsVerified();
            area.AwaitingVerification = verificationResponse.IsPending();

            if (!verificationResponse.IsFailed())
            {
                area.AreaSpecificEmail = request.EmailAddress;
            }

            await configurationRepository.CommitChangesAsync();
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