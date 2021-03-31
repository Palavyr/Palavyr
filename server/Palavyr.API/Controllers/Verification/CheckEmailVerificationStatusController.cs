using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.EmailService.Verification;

namespace Palavyr.API.Controllers.Verification
{

    public class CheckEmailVerificationStatusController : PalavyrBaseController
    {
        private readonly EmailVerificationStatus emailVerificationStatus;
        private ILogger<CheckEmailVerificationStatusController> logger;

        public CheckEmailVerificationStatusController(
            EmailVerificationStatus emailVerificationStatus,
            ILogger<CheckEmailVerificationStatusController> logger)
        {
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
        }

        [HttpPost("verification/email/status")]
        public async Task<bool> RequestNewEmailVerification([FromBody] EmailVerificationRequest emailRequest)
        {
            logger.LogDebug($"Checking email status for {emailRequest.EmailAddress}");
            var verificationStatus = await emailVerificationStatus.CheckVerificationStatus(emailRequest.EmailAddress);
            return verificationStatus;
        }
    }
}