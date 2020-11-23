using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DashboardServer.Data;
using EmailService.verification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.receiverTypes;
using Palavyr.API.response;

namespace Palavyr.API.Controllers.Verification
{
    [Route("api")]
    [ApiController]
    public class EmailVerificationController : ControllerBase
    {
        private ILogger<EmailVerificationController> logger;
        private DashContext dashContext;
        private SenderVerification Verifier { get; set; }
        private IAmazonSimpleEmailService client { get; set; }

        private const string Pending = "Pending";
        private const string Success = "Success";
        private const string Failed = "Failed";

        public EmailVerificationController(
            ILogger<EmailVerificationController> logger,
            IAmazonSimpleEmailService client,
            DashContext dashContext)
        {
            this.logger = logger;
            Verifier = new SenderVerification(logger, client);
            this.client = client;
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

            // First check if email is already verified or has attempted to be verified
            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() {emailRequest.EmailAddress}
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

            var found = response.VerificationAttributes.TryGetValue(emailRequest.EmailAddress, out var status);

            bool result;
            if (found)
            {
                switch (status.VerificationStatus.Value)
                {
                    case (Pending):
                        area.AreaSpecificEmail = emailRequest.EmailAddress;
                        area.EmailIsVerified = false;
                        await dashContext.SaveChangesAsync();
                        return EmailVerificationResponse.CreateNew(
                            Pending,
                            "This email is currently pending verification. Please check your inbox (don't forget to check spam!) for an email from AWS with the subject line 'Amazon Web Services – Email Address Verification Request'.",
                            "Email Verification has already been sent"
                        );

                    case (Failed):
                        result = await Verifier.VerifyEmailAddressAsync(emailRequest.EmailAddress);
                        area.AreaSpecificEmail = emailRequest.EmailAddress;
                        area.EmailIsVerified = false;
                        await dashContext.SaveChangesAsync();
                        return EmailVerificationResponse.CreateNew(
                            Pending,
                            $"You have previously submitted this email for verification, however the attempt failed. We've resent the verification email to {emailRequest.EmailAddress}. Please check your inbox (don't forget to check spam!) for an email from AWS with the subject line 'Amazon Web Services – Email Address Verification Request'. The link will expire in 24 hours.",
                            "Email Verification has already been sent"
                        );

                    case (Success):
                        area.AreaSpecificEmail = emailRequest.EmailAddress;
                        area.EmailIsVerified = true;
                        await dashContext.SaveChangesAsync();
                        return EmailVerificationResponse.CreateNew(
                            Success,
                            "This email has already been verified.",
                            $"Email Address {emailRequest.EmailAddress} already verified!");
                    default:
                        throw new Exception($"Status code undetermined: {status.VerificationStatus.Value}");
                }
            }

            // unseen email address - start fresh..
            result = await Verifier.VerifyEmailAddressAsync(emailRequest.EmailAddress);
            if (!result)
                return EmailVerificationResponse.CreateNew(
                    Failed,
                    "Could not submit email verification request to AWS.",
                    "Failed to create Email Verification Request. Please contact info.palavyr@gmail.com"
                );

            area.AreaSpecificEmail = emailRequest.EmailAddress;
            area.EmailIsVerified = false;
            await dashContext.SaveChangesAsync();
            return EmailVerificationResponse.CreateNew(
                Pending,
                "To complete email verification, go to your inbox and look for an email with the subject line 'Amazon Web Services – Email Address Verification Request' and click the verification link. This link will expire in 24 hours.",
                "Email Verification Submitted");
        }
    }
}