using System.Threading.Tasks;
using DashboardServer.Data;
using EmailService.Verification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Verification
{
    [Route("api")]
    [ApiController]
    public class EmailVerificationController : ControllerBase
    {
        private readonly EmailVerificationStatus emailVerificationStatus;
        private ILogger<EmailVerificationController> logger;
        private readonly DashContext dashContext;

        public EmailVerificationController(
            EmailVerificationStatus emailVerificationStatus,
            ILogger<EmailVerificationController> logger,
            DashContext dashContext)
        {
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPost("verification/email/{areaId}")]
        public async Task<EmailVerificationResponse> RequestNewEmailVerification(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromBody] EmailVerificationRequest emailRequest)
        {
            var area = await dashContext.Areas.SingleOrDefaultAsync(row =>
                row.AccountId == accountId && row.AreaIdentifier == areaId);

            var verificationResponse = await emailVerificationStatus.GetVerificationResponse(emailRequest.EmailAddress);
            
            if (verificationResponse.IsPending() | verificationResponse.IsFailed())
            {
                area.EmailIsVerified = false;
            }
            
            if (verificationResponse.IsSuccess())
            {
                area.EmailIsVerified = true;
            }
            
            if (!verificationResponse.IsFailed())
            {
                area.AreaSpecificEmail = emailRequest.EmailAddress;
            }
            await dashContext.SaveChangesAsync();
            return verificationResponse;
        }
    }
}