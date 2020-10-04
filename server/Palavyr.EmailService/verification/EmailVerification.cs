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
            var res = await EmailClient.VerifyEmailAddressAsync(request);
            var result = res.HttpStatusCode == HttpStatusCode.OK;
            _logger.LogDebug($"Email verification result: {result.ToString()}");
            return result;
        }
    }
}