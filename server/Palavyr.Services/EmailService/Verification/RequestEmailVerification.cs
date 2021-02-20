using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;

namespace Palavyr.Services.EmailService.Verification
{
    public interface IRequestEmailVerification
    {
        public Task<bool> VerifyEmailAddressAsync(string emailAddress);

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
            var request = new VerifyEmailAddressRequest()
            {
                EmailAddress = emailAddress
            };
            logger.LogDebug("Attempting to verify email address.");

            bool result;
            try
            {
                var response = await sesClient.VerifyEmailAddressAsync(request);
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
    }
}