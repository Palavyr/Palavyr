using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class GetAreaByIdController : ControllerBase
    {
        private readonly AccountsContext accountContext;
        private readonly DashContext dashContext;
        private IAmazonSimpleEmailService client;
        private ILogger<GetAreaByIdController> logger;

        private const string Pending = "Pending";
        private const string Success = "Success";
        private const string Failed = "Failed";

        public GetAreaByIdController(
            AccountsContext accountContext,
            DashContext dashContext,
            IAmazonSimpleEmailService client,
            ILogger<GetAreaByIdController> logger
        )
        {
            this.accountContext = accountContext;
            this.dashContext = dashContext;
            this.client = client;
            this.logger = logger;
        }
        
        ///https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html
        /// The verification status of an email address is "Pending" until the email address owner clicks the link within the verification email that Amazon SES sent to that address. If the email address owner clicks the link within 24 hours, the verification status of the email address changes to "Success". If the link is not clicked within 24 hours, the verification status changes to "Failed." In that case, if you still want to verify the email address, you must restart the verification process from the beginning.

        [HttpGet("areas/{areaId}")]
        public async Task<IActionResult> Get([FromHeader] string accountId, string areaId)
        {
            // check if email is verified, if it is not,
            // look up from AWS if email is under verification and
            // set the appropriate  (unmapped property) property

            var data = dashContext.Areas.Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId);

            // if area email not verified, check verification status and attach to response
            if (data.EmailIsVerified)
            {
                data.AwaitingVerification = false;
                return Ok(data);
            }

            if (string.IsNullOrWhiteSpace(data.AreaSpecificEmail))
            {
                // TODO: rethink this -- on account setup and new area creation, we should automatically have 
                // set the area emails to the default email. This is a safeguard measure to ensure we don't 
                // try to query a null or empty email to aws
                var account = await accountContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
                if (account == null) throw new NullReferenceException("Account doesnt exist: Area Data Controller");
                data.AreaSpecificEmail = account.EmailAddress;
            }

            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() {data.AreaSpecificEmail}
            };
            GetIdentityVerificationAttributesResponse response;
            try
            {
                response = await client.GetIdentityVerificationAttributesAsync(identityRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve AWS identities: {ex.Message}");
            }

            var found = response.VerificationAttributes.TryGetValue(data.AreaSpecificEmail, out var status);
            if (found)
            {
                switch (status.VerificationStatus.Value)
                {
                    case (Pending):
                        data.AwaitingVerification = true;
                        data.EmailIsVerified = false;
                        break;

                    case (Failed):
                        data.AwaitingVerification = false;
                        data.EmailIsVerified = false;
                        break;

                    case (Success):
                        data.AwaitingVerification = false;
                        data.EmailIsVerified = true;
                        break;
                }
            }
            else
            {
                data.AwaitingVerification = false;
                data.EmailIsVerified = false;
            }

            await dashContext.SaveChangesAsync();
            return Ok(data);
        }
    }
}