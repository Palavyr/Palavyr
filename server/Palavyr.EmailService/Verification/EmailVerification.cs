using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;

namespace EmailService.verification
{
    public class SenderVerification
    {
        private ILogger logger;
        private IAmazonSimpleEmailService sesClient;

        public SenderVerification(ILogger logger, IAmazonSimpleEmailService sesClient)
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
                result = false;
            }
            logger.LogDebug($"Email verification sent: {result.ToString()}");
            return result;
        }
    }
}