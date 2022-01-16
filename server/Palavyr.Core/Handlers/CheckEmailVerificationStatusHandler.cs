using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.EmailService.Verification;

namespace Palavyr.Core.Handlers
{
    public class CheckEmailVerificationStatusHandler : IRequestHandler<CheckEmailVerificationStatusRequest, CheckEmailVerificationStatusResponse>
    {
        private readonly IEmailVerificationStatus emailVerificationStatus;
        private readonly ILogger<CheckEmailVerificationStatusHandler> logger;

        public CheckEmailVerificationStatusHandler(
            IEmailVerificationStatus emailVerificationStatus,
            ILogger<CheckEmailVerificationStatusHandler> logger)
        {
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
        }

        public async Task<CheckEmailVerificationStatusResponse> Handle(CheckEmailVerificationStatusRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Checking email status for {request.EmailAddress}");
            var verificationStatus = await emailVerificationStatus.CheckVerificationStatus(request.EmailAddress);
            return new CheckEmailVerificationStatusResponse(verificationStatus);
        }
    }

    public class CheckEmailVerificationStatusResponse
    {
        public CheckEmailVerificationStatusResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class CheckEmailVerificationStatusRequest : IRequest<CheckEmailVerificationStatusResponse>
    {
        public string EmailAddress { get; set; }
    }
}