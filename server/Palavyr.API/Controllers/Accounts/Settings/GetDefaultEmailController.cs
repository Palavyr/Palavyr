using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Response;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Route("api")]
    [ApiController]
    public class GetDefaultEmailController : ControllerBase
    {
        private const string Pending = "Pending";
        private const string Success = "Success";
        private const string Failed = "Failed";
        
        private AccountsContext accountsContext;
        private ILogger<GetDefaultEmailController> logger;
        private IAmazonS3 s3Client;
        private IAmazonSimpleEmailService client;

        public GetDefaultEmailController(
            IAmazonS3 s3Client,
            IAmazonSimpleEmailService client,
            AccountsContext accountsContext, 
            ILogger<GetDefaultEmailController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.s3Client = s3Client;
            this.client = client;
        }

        [HttpGet("account/settings/email")]
        public async Task<IActionResult> Get([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);

            if (account.DefaultEmailIsVerified)
            {
                return Ok(AccountEmailSettingsResponse.CreateNew(
                    account.EmailAddress,
                    account.DefaultEmailIsVerified,
                    false
                ));
            }

            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() {account.EmailAddress}
            };

            GetIdentityVerificationAttributesResponse response;
            try
            {
                response = await client.GetIdentityVerificationAttributesAsync(identityRequest);
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Failed to retrieve AWS identities: {ex.Message}");
                return BadRequest();
            }

            var found = response.VerificationAttributes.TryGetValue(account.EmailAddress, out var status);
            if (!found) return Ok(AccountEmailSettingsResponse.CreateNew(account.EmailAddress, false, false));

            bool awaitingVerification;
            switch (status.VerificationStatus.Value)
            {
                case (Pending):
                    awaitingVerification = true;
                    account.DefaultEmailIsVerified = false;
                    await accountsContext.SaveChangesAsync();
                    break;

                case (Failed):
                    awaitingVerification = false;
                    account.DefaultEmailIsVerified = false;
                    await accountsContext.SaveChangesAsync();
                    break;

                case (Success):
                    awaitingVerification = false;
                    account.DefaultEmailIsVerified = true;
                    await accountsContext.SaveChangesAsync();
                    break;
                default:
                    logger.LogDebug("Verification status not recognized.");
                    throw new Exception("Verification status not recognized.");
            }

            return Ok(AccountEmailSettingsResponse.CreateNew(account.EmailAddress, false, awaitingVerification));
        }
    }
}