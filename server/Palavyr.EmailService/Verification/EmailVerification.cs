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
        private ILogger _logger { get; set; }
        private IAmazonSimpleEmailService EmailClient { get;}

        public SenderVerification(ILogger logger, IAmazonSimpleEmailService client)
        {
            _logger = logger;
            EmailClient = client;
        }

        public async Task<bool> VerifyEmailAddress(string emailAddress)
        {
            var request = new VerifyEmailAddressRequest()
            {
                EmailAddress = emailAddress
            };
            _logger.LogDebug("Attempting to verify email address.");

            bool result;
            try
            {
                var response = await EmailClient.VerifyEmailAddressAsync(request);
                result = response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result = false;
            }
            _logger.LogDebug($"Email verification sent: {result.ToString()}");
            return result;
        }
    }
}