using System;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using EmailService.Verification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class GetAreaByIdController : ControllerBase
    {
        private readonly AccountsContext accountContext;
        private readonly DashContext dashContext;
        private readonly EmailVerificationStatus emailVerificationStatus;
        private ILogger<GetAreaByIdController> logger;

        public GetAreaByIdController(
            AccountsContext accountContext,
            DashContext dashContext,
            EmailVerificationStatus emailVerificationStatus,
            ILogger<GetAreaByIdController> logger
        )
        {
            this.accountContext = accountContext;
            this.dashContext = dashContext;
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
        }

        ///https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html
        /// The verification status of an email address is "Pending" until the email address owner clicks the link within the verification email that Amazon SES sent to that address. If the email address owner clicks the link within 24 hours, the verification status of the email address changes to "Success". If the link is not clicked within 24 hours, the verification status changes to "Failed." In that case, if you still want to verify the email address, you must restart the verification process from the beginning.
        [HttpGet("areas/{areaId}")]
        public async Task<Area> Get([FromHeader] string accountId, string areaId)
        {
            var area = dashContext.Areas.Where(row => row.AccountId == accountId).Single(row => row.AreaIdentifier == areaId);

            if (string.IsNullOrWhiteSpace(area.AreaSpecificEmail))
            {
                var account = await accountContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
                if (account == null) throw new NullReferenceException("Account doesnt exist: Area Data Controller");
                area.AreaSpecificEmail = account.EmailAddress;
                await dashContext.SaveChangesAsync();
            }

            var (found, status) = await emailVerificationStatus.RequestEmailVerificationStatus(area.AreaSpecificEmail);
            if (!found)
            {
                throw new Exception("Default email not found. Account is corrupted.");
            }

            var statusResponse = emailVerificationStatus.HandleFoundEmail(status, area.AreaSpecificEmail);

            area.EmailIsVerified = statusResponse.IsVerified();
            area.AwaitingVerification = statusResponse.IsPending();

            await dashContext.SaveChangesAsync();
            return area;
        }
    }
}