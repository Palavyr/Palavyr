using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Services.StripeServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyDefaultEmailAddressHandler : IRequestHandler<ModifyDefaultEmailAddressRequest, ModifyDefaultEmailAddressResponse>
    {
        private const string Pending = "Pending";
        private const string Success = "Success";
        private const string Failed = "Failed";

        private ILogger<ModifyDefaultEmailAddressHandler> logger;
        private readonly IRequestEmailVerification requestEmailVerification;
        private readonly IConfigurationEntityStore<Account> accountStore;
        private IAmazonSimpleEmailService sesClient;
        private StripeCustomerService stripeCustomerService;

        public ModifyDefaultEmailAddressHandler(
            StripeCustomerService stripeCustomerService,
            ILogger<ModifyDefaultEmailAddressHandler> logger,
            IAmazonSimpleEmailService sesClient,
            IRequestEmailVerification requestEmailVerification,
            IConfigurationEntityStore<Account> accountStore
        )
        {
            this.stripeCustomerService = stripeCustomerService;
            this.logger = logger;
            this.sesClient = sesClient;
            this.requestEmailVerification = requestEmailVerification;
            this.accountStore = accountStore;
        }

        public async Task<ModifyDefaultEmailAddressResponse> Handle(ModifyDefaultEmailAddressRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();

            // First check if email is already verified or has attempted to be verified
            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() { request.EmailAddress }
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

            var found = response.VerificationAttributes.TryGetValue(request.EmailAddress, out var status);

            // stripes knowledge of the customer email doesn't have to be in sync with the palavyr understanding.
            // If the customer email isn't verified, we still update stripe. If it remains unverified, then the
            // customer can't use the palavyr. If they change to another email, then stripe will be updated the same
            // and they can try to verify that email. Either way, we should update the stripe email to the current
            // palavyr (verified or unverified email).
            var stripeCustomer = await stripeCustomerService.UpdateStripeCustomerEmail(request.EmailAddress, account.StripeCustomerId);
            if (stripeCustomer == null)
            {
                throw new DomainException("Stripe customer is null - this is not allowed. Please contact Palavyr Support.");
            }

            EmailVerificationResponse verificationResponse;
            bool result;
            if (found)
            {
                // don't need to do anything more with the customer
                switch (status.VerificationStatus.Value)
                {
                    case (Pending):
                        account.EmailAddress = request.EmailAddress;
                        account.DefaultEmailIsVerified = false;
                        verificationResponse = EmailVerificationResponse.CreateNew(
                            Pending,
                            "This email is currently pending verification. Please check your inbox (don't forget to check spam!) for an email from AWS with the subject line 'Amazon Web Services – Email Address Verification Request'.",
                            "Email Verification has already been sent"
                        );
                        break;

                    case (Failed):
                        result = await requestEmailVerification.VerifyEmailAddressAsync(request.EmailAddress);
                        account.EmailAddress = request.EmailAddress;
                        account.DefaultEmailIsVerified = false;
                        verificationResponse = EmailVerificationResponse.CreateNew(
                            Pending,
                            $"You have previously submitted this email for verification, however the attempt failed. We've resent the verification email to {request.EmailAddress}. Please check your inbox (don't forget to check spam!) for an email from AWS with the subject line 'Amazon Web Services – Email Address Verification Request'. The link will expire in 24 hours.",
                            "Email Verification has already been sent"
                        );
                        break;

                    case (Success):
                        account.EmailAddress = request.EmailAddress;
                        account.DefaultEmailIsVerified = true;
                        verificationResponse = EmailVerificationResponse.CreateNew(
                            Success,
                            "This email has already been verified.",
                            $"Email Address {request.EmailAddress} already verified!");
                        break;
                    default:
                        throw new Exception($"Status code undetermined: {status.VerificationStatus.Value}");
                }

                return new ModifyDefaultEmailAddressResponse(verificationResponse);
            }

            // unseen email address - start fresh.
            result = await requestEmailVerification.VerifyEmailAddressAsync(request.EmailAddress);
            if (!result)
            {
                verificationResponse = EmailVerificationResponse.CreateNew(
                    Failed,
                    "Could not submit email verification request the email service provider.",
                    "Failed to create Email Verification Request. Please contact info.palavyr@gmail.com"
                );
                return new ModifyDefaultEmailAddressResponse(verificationResponse);
            }

            account.EmailAddress = request.EmailAddress;
            account.DefaultEmailIsVerified = false;

            verificationResponse = EmailVerificationResponse.CreateNew(
                Pending,
                "To complete email verification, go to your inbox and look for an email with the subject line 'Amazon Web Services – Email Address Verification Request' and click the verification link. This link will expire in 24 hours.",
                "Email Verification Submitted");
            return new ModifyDefaultEmailAddressResponse(verificationResponse);
        }
    }

    public class ModifyDefaultEmailAddressResponse
    {
        public ModifyDefaultEmailAddressResponse(EmailVerificationResponse response) => Response = response;
        public EmailVerificationResponse Response { get; set; }
    }

    public class ModifyDefaultEmailAddressRequest : IRequest<ModifyDefaultEmailAddressResponse>
    {
        public string EmailAddress { get; set; }
    }
}