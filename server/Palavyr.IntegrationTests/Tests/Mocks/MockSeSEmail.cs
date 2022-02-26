using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;

namespace Palavyr.IntegrationTests.Tests.Mocks
{
    public interface IGetEmailSent
    {
        string GetSentText();
        string GetSentHtml();
    }

    public class MockSeSEmail : ISesEmail, IGetEmailSent
    {
        public string SentText { get; set; }
        public string SentHtml { get; set; }

        public string GetSentText()
        {
            return SentText;
        }

        public string GetSentHtml()
        {
            return SentHtml;
        }

        public async Task<bool> SendEmail(string fromAddress, string toAddress, string subject, string htmlBody, string textBody)
        {
            await Task.CompletedTask;
            SentText = textBody;
            SentHtml = htmlBody;
            return true;
        }

        public async Task<bool> SendEmailWithAttachments(
            string fromAddress,
            string toAddress,
            string subject,
            string htmlBody,
            string textBody,
            List<string> filePaths,
            string fromAddressLabel = "",
            string toAddressLabel = "")
        {
            await Task.CompletedTask;
            SentText = textBody;
            SentHtml = htmlBody;
            return true;
        }
    }
}