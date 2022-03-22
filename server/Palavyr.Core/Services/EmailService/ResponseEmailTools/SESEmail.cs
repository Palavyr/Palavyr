using Amazon.SimpleEmail;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.EmailService.ResponseEmailTools
{
    public partial class SesEmail : ISesEmail
    {
        private readonly ILogger<SesEmail> logger;
        private IAmazonSimpleEmailService EmailClient { get; }

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