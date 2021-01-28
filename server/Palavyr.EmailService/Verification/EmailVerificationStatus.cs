using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace EmailService.Verification
{
    public class EmailVerificationStatus
    {
        private readonly IRequestEmailVerification requestEmailVerification;
        private readonly IAmazonSimpleEmailService emailClient;

        public EmailVerificationStatus(
            IRequestEmailVerification requestEmailVerification,
            IAmazonSimpleEmailService emailClient
        )
        {
            this.requestEmailVerification = requestEmailVerification;
            this.emailClient = emailClient;
        }

        async Task<(bool, IdentityVerificationAttributes)> RequestEmailVerificationStatus(string emailAddress)
        {
            // First check if email is already verified or has attempted to be verified
            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() {emailAddress},
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

        public async Task<bool> CheckVerificationStatus(string emailAddress)
        {
            var (found, status) = await RequestEmailVerificationStatus(emailAddress);
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
        
        public async Task<EmailVerificationResponse> GetVerificationResponse(string emailAddress)
        {
            var (found, status) = await RequestEmailVerificationStatus(emailAddress);

            bool result;
            if (found)
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

            // unseen email address - start fresh..
            result = await requestEmailVerification.VerifyEmailAddressAsync(emailAddress);
            if (!result)
                return EmailVerificationResponse.CreateFailed(
                    "Could not submit email verification request to AWS.",
                    "Failed to create Email Verification Request. Please contact info.palavyr@gmail.com"
                );

            return EmailVerificationResponse.CreatePending(
                "To complete email verification, go to your inbox and look for an email with the subject line 'Amazon Web Services – Email Address Verification Request' and click the verification link. This link will expire in 24 hours.",
                "Email Verification Submitted");
        }
    }
}