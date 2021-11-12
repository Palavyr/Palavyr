using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Palavyr.Core.Services.EmailService.SmtpEmail;

namespace Palavyr.API.Controllers.Accounts.Develop
{
    public class SendTestEmailController : PalavyrBaseController
    {
        private readonly ISmtpEmailClient client;
        private readonly IAmazonSimpleEmailService amazonSimpleEmailService;

        public SendTestEmailController(ISmtpEmailClient client, IAmazonSimpleEmailService amazonSimpleEmailService)
        {
            this.client = client;
            this.amazonSimpleEmailService = amazonSimpleEmailService;
        }

        [AllowAnonymous, Microsoft.AspNetCore.Mvc.HttpPost("send-test-email")]
        public async Task Post()
        {
            await client.SendSmtpEmail(
                "paul.e.gradie@gmail.com",
                "paul.e.gradie@gmail.com",
                "TEST EMAIL LOCAL",
                "<h2>TEST</h2>",
                "Okay");
        }

        [AllowAnonymous, Microsoft.AspNetCore.Mvc.HttpPost("get-test-identities")]
        public async Task PostIdent()
        {
            var identityRequest = new GetIdentityVerificationAttributesRequest()
            {
                Identities = new List<string>() {"Paul.e.gradie@gmail.com"},
            };
            GetIdentityVerificationAttributesResponse response;
            try
            {
                response = await amazonSimpleEmailService.GetIdentityVerificationAttributesAsync(identityRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve AWS identities: {ex.Message}");
            }
        }
    }
}