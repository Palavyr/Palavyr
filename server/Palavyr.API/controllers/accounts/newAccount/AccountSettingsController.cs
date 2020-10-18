using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DashboardServer.Data;
using EmailService.verification;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.API.receiverTypes;
using Palavyr.API.ReceiverTypes;
using Palavyr.API.response;
using Server.Domain.AccountDB;

namespace Palavyr.API.controllers.accounts.newAccount
{
    [Route("api/account/settings")]
    [ApiController]
    public class AccountSettings : BaseController
    {
        private const string Pending = "Pending";
        private const string Success = "Success";
        private const string Failed = "Failed";
        private IAmazonSimpleEmailService _client { get; set; }
        private static ILogger<AccountSettings> _logger;
        private SenderVerification Verifier { get; set; }

        public AccountSettings(
            ILogger<AccountSettings> logger,
            IAmazonSimpleEmailService client,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;
            Verifier = new SenderVerification(logger, client);
            _client = client;
        }

        [HttpPut("update/password")]
        public bool UpdatePassword([FromHeader] string accountId, AccountDetails accountDetails)
        {
            var account = AccountContext
                .Accounts
                .Single(row => row.AccountId == accountId);

            var oldHashedPassword = accountDetails.OldPassword;
            if (oldHashedPassword != accountDetails.Password)
            {
                return false;
            }

            account.Password = accountDetails.Password;
            AccountContext.SaveChanges();
            return true;
        }

        [HttpPut("update/email")]
        public async Task<EmailVerificationResponse> UpdateEmail([FromHeader] string accountId,
            [FromBody] EmailVerificationRequest emailRequest)
        {
            var account = AccountContext
                .Accounts
                .Single(row => row.AccountId == accountId);

            // First check if email is already verified or has attempted to be verified
            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() {emailRequest.EmailAddress}
            };
            GetIdentityVerificationAttributesResponse response;
            try
            {
                response = await _client.GetIdentityVerificationAttributesAsync(identityRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve AWS identities: {ex.Message}");
            }

            var found = response.VerificationAttributes.TryGetValue(emailRequest.EmailAddress, out var status);

            bool result;
            if (found)
            {
                switch (status.VerificationStatus.Value)
                {
                    case (Pending):
                        account.EmailAddress = emailRequest.EmailAddress;
                        account.DefaultEmailIsVerified = false;
                        await AccountContext.SaveChangesAsync();
                        return EmailVerificationResponse.CreateNew(
                            Pending,
                            "This email is currently pending verification. Please check your inbox (don't forget to check spam!) for an email from AWS with the subject line 'Amazon Web Services – Email Address Verification Request'.",
                            "Email Verification has already been sent"
                        );

                    case (Failed):
                        result = await Verifier.VerifyEmailAddress(emailRequest.EmailAddress);
                        account.EmailAddress = emailRequest.EmailAddress;
                        account.DefaultEmailIsVerified = false;
                        await AccountContext.SaveChangesAsync();
                        return EmailVerificationResponse.CreateNew(
                            Pending,
                            $"You have previously submitted this email for verification, however the attempt failed. We've resent the verification email to {emailRequest.EmailAddress}. Please check your inbox (don't forget to check spam!) for an email from AWS with the subject line 'Amazon Web Services – Email Address Verification Request'. The link will expire in 24 hours.",
                            "Email Verification has already been sent"
                        );

                    case (Success):
                        account.EmailAddress = emailRequest.EmailAddress;
                        account.DefaultEmailIsVerified = true;
                        AccountContext.SaveChangesAsync();
                        return EmailVerificationResponse.CreateNew(
                            Success,
                            "This email has already been verified.",
                            $"Email Address {emailRequest.EmailAddress} already verified!");
                    default:
                        throw new Exception($"Status code undetermined: {status.VerificationStatus.Value}");
                }
            }

            // unseen email address - start fresh.
            result = await Verifier.VerifyEmailAddress(emailRequest.EmailAddress);
            if (!result)
                return EmailVerificationResponse.CreateNew(
                    Failed,
                    "Could not submit email verification request to AWS.",
                    "Failed to create Email Verification Request. Please contact info.palavyr@gmail.com"
                );

            account.EmailAddress = emailRequest.EmailAddress;
            account.DefaultEmailIsVerified = false;
            AccountContext.SaveChangesAsync();
            return EmailVerificationResponse.CreateNew(
                Pending,
                "To complete email verification, go to your inbox and look for an email with the subject line 'Amazon Web Services – Email Address Verification Request' and click the verification link. This link will expire in 24 hours.",
                "Email Verification Submitted");
        }

        [HttpPut("update/companyname")]
        public StatusCodeResult UpdateCompanyName([FromHeader] string accountId, LoginCredentials login)
        {
            var account = AccountContext
                .Accounts
                .Single(row => row.AccountId == accountId);
            account.CompanyName = login.CompanyName;
            AccountContext.SaveChanges();
            return new OkResult();
        }

        [HttpPut("update/username")]
        public StatusCodeResult UpdateUserName([FromHeader] string accountId, LoginCredentials login)
        {
            var allUserNames = AccountContext.Accounts.ToList().Select(x => x.UserName);
            if (allUserNames.Contains(login.Username))
            {
                return new BadRequestResult();
            }

            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            account.UserName = login.Username;
            AccountContext.SaveChanges();
            return new OkResult();
        }

        [HttpPut("update/phonenumber")]
        public StatusCodeResult UpdatePhoneNumber([FromHeader] string accountId, LoginCredentials login)
        {
            var account = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId);
            account.PhoneNumber = login.PhoneNumber;
            AccountContext.SaveChanges();
            return new OkResult();
        }

        [HttpPut("update/locale")]
        public StatusCodeResult UpdateLocale([FromHeader] string accountId, AccountDetails accountDetails)
        {
            var account = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId);
            account.Locale = accountDetails.Locale;
            AccountContext.SaveChanges();
            return new OkResult();
        }

        [HttpGet("locale")]
        public string GetLocale([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId);
            return account.Locale ?? "";
        }

        [HttpGet("companyname")]
        public string GetCompanyName([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            return account.CompanyName ?? "";
        }

        [HttpGet("email")]
        public async Task<AccountEmailSettingsResponse> GetEmail([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);

            if (account.DefaultEmailIsVerified)
            {
                return AccountEmailSettingsResponse.CreateNew(
                    account.EmailAddress,
                    account.DefaultEmailIsVerified,
                    false
                );
            }

            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() {account.EmailAddress}
            };
            
            GetIdentityVerificationAttributesResponse response;
            try
            {
                response = await _client.GetIdentityVerificationAttributesAsync(identityRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve AWS identities: {ex.Message}");
            }

            var found = response.VerificationAttributes.TryGetValue(account.EmailAddress, out var status);
            if (!found) return AccountEmailSettingsResponse.CreateNew(account.EmailAddress, false, false);

            bool awaitingVerification;
            switch (status.VerificationStatus.Value)
            {
                case (Pending):
                    awaitingVerification = true;
                    account.DefaultEmailIsVerified = false;
                    await AccountContext.SaveChangesAsync();
                    break;

                case (Failed):
                    awaitingVerification = false;
                    account.DefaultEmailIsVerified = false;
                    await AccountContext.SaveChangesAsync();
                    break;

                case (Success):
                    awaitingVerification = false;
                    account.DefaultEmailIsVerified = true;
                    await AccountContext.SaveChangesAsync();
                    break;
                default:
                    throw new Exception("Verification status not recognized.");
            }

            return AccountEmailSettingsResponse.CreateNew(account.EmailAddress, false, awaitingVerification);
        }

        [HttpGet("username")]
        public string GetUserName([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            return account.UserName ?? "";
        }

        [HttpGet("phonenumber")]
        public string GetPhoneNumber([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            return account.PhoneNumber ?? "";
        }

        [HttpGet("apikey")]
        public string GetApiKey([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            return account.ApiKey ?? "";
        }
    }
}