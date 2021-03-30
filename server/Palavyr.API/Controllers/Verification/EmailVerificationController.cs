using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.EmailService.Verification;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.Verification
{

    public class EmailVerificationController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly EmailVerificationStatus emailVerificationStatus;
        private ILogger<EmailVerificationController> logger;

        public EmailVerificationController(
            IConfigurationRepository configurationRepository,
            EmailVerificationStatus emailVerificationStatus,
            ILogger<EmailVerificationController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
        }

        [HttpPost("verification/email/{areaId}")]
        public async Task<EmailVerificationResponse> RequestNewEmailVerification(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromBody] EmailVerificationRequest emailRequest
        )
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);
            var verificationResponse = await emailVerificationStatus.GetVerificationResponse(emailRequest.EmailAddress);

            area.EmailIsVerified = verificationResponse.IsVerified();
            area.AwaitingVerification = verificationResponse.IsPending();

            if (!verificationResponse.IsFailed())
            {
                area.AreaSpecificEmail = emailRequest.EmailAddress;
            }

            await configurationRepository.CommitChangesAsync();
            return verificationResponse;
        }
    }
}