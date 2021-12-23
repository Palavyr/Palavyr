using Amazon.SimpleEmail;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Services.EmailService.SmtpEmail;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.EmailService.ResponseEmailTools
{
    public partial class SesEmail : ISesEmail
    {
        private readonly ILogger<SesEmail> logger;
        private readonly IDetermineCurrentOperatingSystem determineCurrentOperatingSystem;
        private readonly ISmtpEmailClient smtpEmailClient;
        private readonly ITransportACancellationToken cancellationTokenTransport;
        private IAmazonSimpleEmailService EmailClient { get;}
        
        public SesEmail(
            ILogger<SesEmail> logger,
            IAmazonSimpleEmailService client,
            IDetermineCurrentOperatingSystem determineCurrentOperatingSystem,
            ISmtpEmailClient smtpEmailClient,
            ITransportACancellationToken cancellationTokenTransport
        )
        {
            this.logger = logger;
            this.determineCurrentOperatingSystem = determineCurrentOperatingSystem;
            this.smtpEmailClient = smtpEmailClient;
            this.cancellationTokenTransport = cancellationTokenTransport;
            EmailClient = client;
        }
    }
}