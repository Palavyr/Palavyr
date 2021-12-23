using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.EmailService.Verification;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetDefaultEmailController : PalavyrBaseController
    {
        private readonly IEmailVerificationStatus emailVerificationStatus;
        private readonly IAccountRepository accountRepository;

        public GetDefaultEmailController(
            IEmailVerificationStatus emailVerificationStatus,
            IAccountRepository accountRepository
        )
        {
            this.emailVerificationStatus = emailVerificationStatus;
            this.accountRepository = accountRepository;
        }

        [HttpGet("account/settings/email")]
        public async Task<AccountEmailSettingsResponse> Get()
        {
            var account = await accountRepository.GetAccount();
            if (string.IsNullOrWhiteSpace(account.EmailAddress)) throw new Exception($"Default email for account id {account.AccountId} not found. Account corruption.");

            var verificationResponse = await emailVerificationStatus.GetVerificationResponse(account.EmailAddress);

            account.DefaultEmailIsVerified = verificationResponse.IsVerified();
            await accountRepository.CommitChangesAsync();

            return AccountEmailSettingsResponse.CreateNew(
                account.EmailAddress,
                verificationResponse.IsVerified(),
                verificationResponse.IsPending()
            );
        }
    }
}