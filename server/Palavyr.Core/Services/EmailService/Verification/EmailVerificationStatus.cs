using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace Palavyr.Core.Services.EmailService.Verification
{
    public interface ICloudEmailService
    {
        Task<(bool, IdentityVerificationAttributes)> CheckEmailVerificationStatus(string emailAddress);
    }
    
    public class CloudEmailService : ICloudEmailService
    {
        private readonly IAmazonSimpleEmailService emailClient;

        public CloudEmailService(IAmazonSimpleEmailService emailClient)
        {
            this.emailClient = emailClient;
        }

        public async Task<(bool, IdentityVerificationAttributes)> CheckEmailVerificationStatus(string emailAddress)
        {
            // First check if email is already verified or has attempted to be verified
            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() { emailAddress },
            };
            GetIdentityVerificationAttributesResponse response;
            try
            {
                response = await emailClient.GetIdentityVerificationAttributesAsync(identityRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve AWS identities: {ex.Message}");
            }

            var found = response.VerificationAttributes.TryGetValue(emailAddress, out var status);

            return (found, status);
        }
    }


    public class EmailVerificationStatus : IEmailVerificationStatus
    {
        private readonly IRequestEmailVerification requestEmailVerification;
        private readonly ICloudEmailService emailClient;

        public EmailVerificationStatus(
            IRequestEmailVerification requestEmailVerification,
            ICloudEmailService emailClient
        )
        {
            this.requestEmailVerification = requestEmailVerification;
            this.emailClient = emailClient;
        }

        public async Task<(bool, IdentityVerificationAttributes)> RequestEmailVerificationStatus(string emailAddress)
        {
            return await emailClient.CheckEmailVerificationStatus(emailAddress);

        }

        public async Task<bool> CheckVerificationStatus(string emailAddress)
        {
            var (found, status) = await emailClient.CheckEmailVerificationStatus(emailAddress);
            if (found)
            {
                switch (status.VerificationStatus.Value)
                {
                    case (EmailVerificationResponse.Success):
                        return true;
                    case (EmailVerificationResponse.Pending):
                        return false;
                    case (EmailVerificationResponse.Failed):
                        return false;
                    default:
                        return false;
                }
            }

            return false;
        }

        public EmailVerificationResponse HandleFoundEmail(IdentityVerificationAttributes status, string emailAddress)
        {
            switch (status.VerificationStatus.Value)
            {
                case (EmailVerificationResponse.Pending):
                    return EmailVerificationResponse.CreatePending(
                        "This email is currently pending verification. Please check your inbox (don't forget to check spam!) for an email from AWS with the subject line 'Amazon Web Services – Email Address Verification Request'.",
                        "Email Verification has already been sent"
                    );

                case (EmailVerificationResponse.Failed):
                    return EmailVerificationResponse.CreatePending(
                        $"You have previously submitted this email for verification, however the attempt failed. We've resent the verification email to {emailAddress}. Please check your inbox (don't forget to check spam!) for an email from AWS with the subject line 'Amazon Web Services – Email Address Verification Request'. The link will expire in 24 hours.",
                        "Email Verification has already been sent"
                    );

                case (EmailVerificationResponse.Success):
                    return EmailVerificationResponse.CreateIsVerified(
                        "This email has already been verified.",
                        $"Email Address {emailAddress} already verified!");
                default:
                    throw new Exception($"Status code undetermined: {status.VerificationStatus.Value}");
            }
        }

        public async Task<EmailVerificationResponse> HandleNotFoundEmail(IdentityVerificationAttributes status, string emailAddress)
        {
            var result = await requestEmailVerification.VerifyEmailAddressAsync(emailAddress);
            if (!result)
                return EmailVerificationResponse.CreateFailed(
                    "Could not submit email verification request to the email service provider.",
                    "Failed to create Email Verification Request. Please contact info.palavyr@gmail.com"
                );

            return EmailVerificationResponse.CreatePending(
                "To complete email verification, go to your inbox and look for an email with the subject line 'Amazon Web Services – Email Address Verification Request' and click the verification link. This link will expire in 24 hours.",
                "Email Verification Submitted");
        }


        public async Task<EmailVerificationResponse> GetVerificationResponse(string emailAddress)
        {
            var (found, status) = await RequestEmailVerificationStatus(emailAddress);

            if (found)
            {
                return HandleFoundEmail(status, emailAddress);
            }

            return await HandleNotFoundEmail(status, emailAddress);
        }
    }
}