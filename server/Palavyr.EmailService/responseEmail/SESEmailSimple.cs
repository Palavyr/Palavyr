using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;

namespace EmailService
{
    public partial class SESEmail
    {
        private readonly ILogger _logger;

        private IAmazonSimpleEmailService EmailClient { get;}
        
        public SESEmail(
            ILogger _logger,
            IAmazonSimpleEmailService client
            )
        {
            this._logger = _logger;
            EmailClient = client;
        }

        
        private Body CreatePlainBody(string htmlBody, string textBody)
        {
            return new Body
            {
                Html = new Content
                {
                    Charset = "UTF-8",
                    Data = htmlBody
                },
                Text = new Content
                {
                    Charset = "UTF-8",
                    Data = textBody
                }
            };
        }
        public async Task<bool> SendEmail(string fromAddress, string toAddress, string subject, string htmlBody, string textBody)
        {
            
            var sendRequest = new SendEmailRequest()
            {
                Source = fromAddress,
                Destination = new Destination
                {
                    ToAddresses = new List<string> {toAddress}
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = CreatePlainBody(htmlBody, textBody)
                },
                
            };

            try
            {
                _logger.LogDebug("Sending Email...");
                await EmailClient.SendEmailAsync(sendRequest);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Email was not sent. ");
                _logger.LogDebug("Error: " + ex.Message);
                //TODO: If this errors, then we need to send a response that the email couldn't be sent, and then record the email in the bounceback DB.
                return false;
            }
        }
    }
}