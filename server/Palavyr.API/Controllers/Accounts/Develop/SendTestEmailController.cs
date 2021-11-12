using System.Threading.Tasks;
using System.Web.Http;
using Palavyr.Core.Services.EmailService.SmtpEmail;

namespace Palavyr.API.Controllers.Accounts.Develop
{
    public class SendTestEmailController : PalavyrBaseController
    {
        private readonly ISmtpEmailClient client;

        public SendTestEmailController(ISmtpEmailClient client)
        {
            this.client = client;
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
    }
}