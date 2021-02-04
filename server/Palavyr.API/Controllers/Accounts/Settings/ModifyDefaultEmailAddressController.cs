using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DashboardServer.Data;
using EmailService.Verification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;
using Palavyr.API.Response;
using Palavyr.API.Services.StripeServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Route("api")]
    [ApiController]
    public class ModifyDefaultEmailAddressController : ControllerBase
    {
        private ILogger<ModifyDefaultEmailAddressController> logger;
        private AccountsContext accountsContext;
        private readonly IRequestEmailVerification requestEmailVerification;
        private IAmazonSimpleEmailService sesClient;
        private StripeCustomerService stripeCustomerService;

        private const string Pending = "Pending";
        private const string Success = "Success";
        private const string Failed = "Failed";
        public ModifyDefaultEmailAddressController(
            AccountsContext accountsContext, 
            StripeCustomerService stripeCustomerService,
            ILogger<ModifyDefaultEmailAddressController> logger,
            IAmazonSimpleEmailService sesClient,
            IRequestEmailVerification requestEmailVerification
        )
        {
            this.logger = logger;
            this.sesClient = sesClient;
            this.accountsContext = accountsContext;
            this.stripeCustomerService = stripeCustomerService;
            this.requestEmailVerification = requestEmailVerification;
        }
        
        [HttpPut("account/settings/email")]
        public async Task<IActionResult> Modify(
            [FromHeader] string accountId,
            [FromBody] EmailVerificationRequest emailRequest
        )
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.AccountId == accountId);

            // First check if email is already verified or has attempted to be verified
            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() {emailRequest.EmailAddress}
            };
            
            GetIdentityVerificationAttributesResponse response;
            try
            {
                response = await sesClient.GetIdentityVerificationAttributesAsync(identityRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve AWS identities: {ex.Message}");
            }

            var found = response.VerificationAttributes.TryGetValue(emailRequest.EmailAddress, out var status);

            // stripes knowledge of the customer email doesn't have to be in sync with the palavyr understanding.
            // If the customer email isn't verified, we still update stripe. If it remains unverified, then the
            // customer can't use the palavyr. If they change to another email, then stripe will be updated the same
            // and they can try to verify that email. Either way, we should update the stripe email to the current
            // palavyr (verified or unverified email).
            var stripeCustomer = await stripeCustomerService.UpdateStripeCustomerEmail(emailRequest.EmailAddress, account.StripeCustomerId);
            if (stripeCustomer == null)
            {
                return BadRequest();
            }
            EmailVerificationResponse verificationResponse;
            bool result;
            if (found)
            {
                // don't need to do anything more with the customer
                switch (status.VerificationStatus.Value)
                {
                    case (Pending):
                        account.EmailAddress = emailRequest.EmailAddress;
                        account.DefaultEmailIsVerified = false;
                        await accountsContext.SaveChangesAsync();
                        verificationResponse = EmailVerificationResponse.CreateNew(
                            Pending,
                            "This email is currently pending verification. Please check your inbox (don't forget to check spam!) for an email from AWS with the subject line 'Amazon Web Services – Email Address Verification Request'.",
                            "Email Verification has already been sent"
                        );
                        break;

                    case (Failed):
                        result = await requestEmailVerification.VerifyEmailAddressAsync(emailRequest.EmailAddress);
                        account.EmailAddress = emailRequest.EmailAddress;
                        account.DefaultEmailIsVerified = false;
                        await accountsContext.SaveChangesAsync();
                        verificationResponse = EmailVerificationResponse.CreateNew(
                            Pending,
                            $"You have previously submitted this email for verification, however the attempt failed. We've resent the verification email to {emailRequest.EmailAddress}. Please check your inbox (don't forget to check spam!) for an email from AWS with the subject line 'Amazon Web Services – Email Address Verification Request'. The link will expire in 24 hours.",
                            "Email Verification has already been sent"
                        );
                        break;

                    case (Success):
                        account.EmailAddress = emailRequest.EmailAddress;
                        account.DefaultEmailIsVerified = true;
                        await accountsContext.SaveChangesAsync();
                        verificationResponse = EmailVerificationResponse.CreateNew(
                            Success,
                            "This email has already been verified.",
                            $"Email Address {emailRequest.EmailAddress} already verified!");
                        break;
                    default:
                        throw new Exception($"Status code undetermined: {status.VerificationStatus.Value}");
                }
                return Ok(verificationResponse);
            }

            // unseen email address - start fresh.
            result = await requestEmailVerification.VerifyEmailAddressAsync(emailRequest.EmailAddress);
            if (!result)
            {
                verificationResponse = EmailVerificationResponse.CreateNew(
                    Failed,
                    "Could not submit email verification request to AWS.",
                    "Failed to create Email Verification Request. Please contact info.palavyr@gmail.com"
                );
                return Ok(verificationResponse);
            }

            account.EmailAddress = emailRequest.EmailAddress;
            account.DefaultEmailIsVerified = false;

            await accountsContext.SaveChangesAsync();
            verificationResponse = EmailVerificationResponse.CreateNew(
                Pending,
                "To complete email verification, go to your inbox and look for an email with the subject line 'Amazon Web Services – Email Address Verification Request' and click the verification link. This link will expire in 24 hours.",
                "Email Verification Submitted");
            return Ok(verificationResponse);
        }
    }
}