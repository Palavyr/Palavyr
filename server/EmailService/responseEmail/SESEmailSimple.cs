using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace EmailService
{
    public partial class SESEmail
    {

        private IAmazonSimpleEmailService EmailClient { get;}

        public SESEmail(IAmazonSimpleEmailService client)
        {
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
                Console.WriteLine("Sending Email...");
                await EmailClient.SendEmailAsync(sendRequest);
                Console.WriteLine("Email Sent Successfully. No bouncebacks.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email was not sent. ");
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}