using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.EmailService.SmtpEmail;

namespace Palavyr.API.Controllers.Testing
{
    public class TestSmtpEmails : PalavyrBaseController
    {
        private readonly ISmtpEmailClient smtpEmailClient;
        private readonly ILogger<TestSmtpEmails> logger;

        public TestSmtpEmails(ISmtpEmailClient smtpEmailClient, ILogger<TestSmtpEmails> logger)
        {
            this.smtpEmailClient = smtpEmailClient;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("test-smtp")]
        public string Post()
        {
            try
            {
                logger.LogDebug("Attempting to send a test email...");
                smtpEmailClient.SendSmtpEmail(
                    "palavyr@gmail.com",
                    "paul.e.gradie@gmail.com",
                    "Test SMTP Email from Paul",
                    "<h1>THIS WORKED</h1>"
                );
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                logger.LogDebug("_------------------------_");
                Console.WriteLine(ex.Message);
                logger.LogDebug(ex.Message);
                throw;
            }
        }
    }
}