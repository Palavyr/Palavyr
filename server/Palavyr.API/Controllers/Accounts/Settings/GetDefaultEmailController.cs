using System;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.EmailService.Verification;

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
        private readonly EmailVerificationStatus emailVerificationStatus;
        private ILogger<GetDefaultEmailController> logger;
        private IAmazonSimpleEmailService client;

        public GetDefaultEmailController(
            EmailVerificationStatus emailVerificationStatus,
            IAmazonSimpleEmailService client,
            AccountsContext accountsContext,
            ILogger<GetDefaultEmailController> logger
        )
        {
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.client = client;
        }

        [HttpGet("account/settings/email")]
        public async Task<AccountEmailSettingsResponse> Get([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            if (string.IsNullOrWhiteSpace(account.EmailAddress)) throw new Exception($"Default email for account id {account.AccountId} not found. Account corruption.");

            var verificationResponse = await emailVerificationStatus.GetVerificationResponse(account.EmailAddress);

            account.DefaultEmailIsVerified = verificationResponse.IsVerified();
            await accountsContext.SaveChangesAsync();

            return AccountEmailSettingsResponse.CreateNew(
                account.EmailAddress,
                verificationResponse.IsVerified(),
                verificationResponse.IsPending()
            );
        }
    }
}