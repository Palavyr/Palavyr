using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;

namespace Palavyr.Core.Services.EmailService.Verification
{
    public interface IRequestEmailVerification
    {
        public Task<bool> VerifyEmailAddressAsync(string emailAddress);
        public Task<bool> DeleteEmailIdentityAsync(string emailAddress);
    }

    public class RequestEmailVerification : IRequestEmailVerification
    {
        private ILogger<RequestEmailVerification> logger;
        private IAmazonSimpleEmailService sesClient;

        public RequestEmailVerification(ILogger<RequestEmailVerification> logger, IAmazonSimpleEmailService sesClient)
        {
            this.logger = logger;
            this.sesClient = sesClient;
        }

        public async Task<bool> VerifyEmailAddressAsync(string emailAddress)
        {
            logger.LogDebug("Attempting to verify email address.");

            bool result;
            try
            {
                var customVerificationRequest = new SendCustomVerificationEmailRequest
                {
                    EmailAddress = emailAddress.ToLowerInvariant(),
                    TemplateName = "DefaultPalavyrVerificationTemplate"
                };

                var response = await sesClient.SendCustomVerificationEmailAsync(customVerificationRequest);
                result = response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Exception: {ex.Message}");
                result = false;
            }

            logger.LogDebug($"Email verification sent: {result.ToString()}");
            return result;
        }

        public async Task<bool> DeleteEmailIdentityAsync(string emailAddress)
        {
            var deletionRequest = new DeleteIdentityRequest()
            {
                Identity = emailAddress.ToLowerInvariant()
            };
            bool result;
            try
            {
                var response = await sesClient.DeleteIdentityAsync(deletionRequest);
                result = response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Exception: {ex.Message}");
                result = false;
            }

            logger.LogDebug($"Identity deletion request sent: {result.ToString()}");
            return result;
        }
    }
}