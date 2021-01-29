using System.Threading.Tasks;
using EmailService.Verification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Verification
{
    [Route("api")]
    [ApiController]
    public class CheckEmailVerificationStatusController : ControllerBase
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