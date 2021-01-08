using Amazon.SimpleEmail;
using Microsoft.Extensions.Logging;
namespace EmailService.ResponseEmail
{
    public partial class SesEmail : ISesEmail
    {
        private readonly ILogger<SesEmail> logger;
        private IAmazonSimpleEmailService EmailClient { get;}
        
        public SesEmail(
            ILogger<SesEmail> logger,
            IAmazonSimpleEmailService client
        )
        {
            this.logger = logger;
            EmailClient = client;
        }
    }
}