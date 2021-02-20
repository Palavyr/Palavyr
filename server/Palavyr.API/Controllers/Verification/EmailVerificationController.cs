using System.Threading.Tasks;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;
using Palavyr.Services.EmailService.Verification;

namespace Palavyr.API.Controllers.Verification
{
    [Route("api")]
    [ApiController]
    public class EmailVerificationController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private readonly EmailVerificationStatus emailVerificationStatus;
        private ILogger<EmailVerificationController> logger;

        public EmailVerificationController(
            IDashConnector dashConnector,
            EmailVerificationStatus emailVerificationStatus,
            ILogger<EmailVerificationController> logger
        )
        {
            this.dashConnector = dashConnector;
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
            var area = await dashConnector.GetAreaById(accountId, areaId);
            var verificationResponse = await emailVerificationStatus.GetVerificationResponse(emailRequest.EmailAddress);

            area.EmailIsVerified = verificationResponse.IsVerified();
            area.AwaitingVerification = verificationResponse.IsPending();

            if (!verificationResponse.IsFailed())
            {
                area.AreaSpecificEmail = emailRequest.EmailAddress;
            }

            await dashConnector.CommitChangesAsync();
            return verificationResponse;
        }
    }
}