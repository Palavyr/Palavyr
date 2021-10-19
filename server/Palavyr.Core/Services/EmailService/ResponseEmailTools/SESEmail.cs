using Amazon.SimpleEmail;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Services.EmailService.SmtpEmail;

namespace Palavyr.Core.Services.EmailService.ResponseEmailTools
{
    public partial class SesEmail : ISesEmail
    {
        private readonly ILogger<SesEmail> logger;
        private readonly IDetermineCurrentOperatingSystem determineCurrentOperatingSystem;
        private readonly ISmtpEmailClient smtpEmailClient;
        private IAmazonSimpleEmailService EmailClient { get;}
        
        public SesEmail(
            ILogger<SesEmail> logger,
            IAmazonSimpleEmailService client,
            IDetermineCurrentOperatingSystem determineCurrentOperatingSystem,
            ISmtpEmailClient smtpEmailClient
        )
        {
            this.logger = logger;
            this.determineCurrentOperatingSystem = determineCurrentOperatingSystem;
            this.smtpEmailClient = smtpEmailClient;
            EmailClient = client;
        }
    }
}