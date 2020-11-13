using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DashboardServer.Data;
using EmailService.verification;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.API.receiverTypes;
using Palavyr.API.ReceiverTypes;
using Palavyr.API.response;
using Palavyr.API.ResponseTypes;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.Common.FileSystem.FormPaths.IO;
using Server.Domain.AccountDB;
using Server.Domain.Accounts;
using Stripe;

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
        private readonly IAmazonS3 _s3Client;
        private readonly IStripeClient _stripeClient = new StripeClient();
        
        public AccountSettings(
            IAmazonS3 s3Client,
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
            _s3Client = s3Client;
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

        public async Task<Customer> UpdateStripeCustomerEmail(string emailAddress, string customerId)
        {
            var options = new CustomerUpdateOptions
            {
               Email = emailAddress
            };
            var service = new CustomerService(_stripeClient);
            var response = await service.UpdateAsync(customerId, options);
            return response;
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

            // stripes knowledge of the customer email doesn't have to be in sync with the palavyr understanding.
            // If the customer email isn't verified, we still update stripe. If it remains unverified, then the
            // customer can't use the palavyr. If they change to another email, then stripe will be updated the same
            // and they can try to verfiy that email. Either way, we should update the stripe email to the current
            // palavyr (verified or unverified email).
            await UpdateStripeCustomerEmail(emailRequest.EmailAddress, account.StripeCustomerId);
            
            bool result;
            if (found)
            {
                // don't need to do anything more with the customer
                
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
                        await AccountContext.SaveChangesAsync();
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

            await AccountContext.SaveChangesAsync();
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

        [HttpPut("update/logo")]
        [ActionName("Decode")]
        public async Task<IActionResult> UpdateCompanyLogo([FromHeader] string accountId,
            [FromForm(Name = "files")] IFormFile file) // will take form data. Check attachments
        {
            // Get the directory where we save the logo images
            var extension = Path.GetExtension(file.FileName);
            var logoDirectory = FormDirectoryPaths.FormLogoImageDir(accountId);

            var files = LogoPaths.ListLogoPathsAsDiskPaths(accountId);
            if (files.Count > 0)
            {
                var dir = new DirectoryInfo(logoDirectory);
                foreach (var fi in dir.GetFiles())
                {
                    fi.Delete();
                }
            }

            var filepath = Path.Combine(logoDirectory, Guid.NewGuid().ToString()) + extension;

            
            await FileIO.SaveFile(filepath, file);
            var account = await AccountContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            account.AccountLogoUri = filepath;
            await AccountContext.SaveChangesAsync();

            var link = await UriUtils.CreateLogoImageLinkAsURI(
                _logger,
                accountId,
                Path.GetFileName(filepath),
                filepath,
                _s3Client
            );
            
            return Ok(link);
        }

        [HttpGet("current-plan")]
        public IActionResult GetCurrentPlan([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId);
            string planStatus;
            switch (account.PlanType)
            {
                case (UserAccount.PlanTypeEnum.Free):
                    planStatus = UserAccount.PlanTypeEnum.Free.ToString();
                    break;
                case (UserAccount.PlanTypeEnum.Premium):
                    planStatus = UserAccount.PlanTypeEnum.Premium.ToString();
                    break;    
                case (UserAccount.PlanTypeEnum.Pro):
                    planStatus = UserAccount.PlanTypeEnum.Pro.ToString();
                    break;
                default:
                    _logger.LogDebug("Plan type was not able to be determined.");
                    throw new Exception("Plan Type not able to be determined.");
            }

            return Ok(planStatus);
        }
        
        [HttpGet("logo")]
        public async Task<IActionResult> GetCompanyLogo([FromHeader] string accountId)
        {
            /// Do I upload an image file, or allow them to use a link?
            /// Only use an actual file for now
            var files = LogoPaths.ListLogoPathsAsDiskPaths(accountId);
            var logoFile = files[0]; // we only allow one logo file. If it changes, we delete it.
            var link = await UriUtils.CreateLogoImageLinkAsURI(
                _logger,
                accountId,
                Path.GetFileName(logoFile),
                logoFile,
                _s3Client
            );
            return Ok(link);
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
        public PhoneDetails GetPhoneNumber([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            var phoneDetails = PhoneDetails.Create(account.PhoneNumber, account.Locale);
            return phoneDetails;
        }

        [HttpGet("apikey")]
        public string GetApiKey([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            return account.ApiKey ?? "";
        }
    }
}