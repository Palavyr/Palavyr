using System.Collections.Generic;
using System.Threading.Tasks;

namespace Palavyr.Core.Services.EmailService.ResponseEmailTools
{
    public interface ISesEmail
    {
        Task<bool> SendEmail(
            string fromAddress,
            string toAddress,
            string subject,
            string htmlBody,
            string textBody,
            bool notifyIntentOwner = false);

        Task<bool> SendEmailWithAttachments(
            string fromAddress,
            string toAddress,
            string subject,
            string htmlBody,
            string textBody,
            List<string> filePaths,
            string fromAddressLabel = "",
            string toAddressLabel = "",
            bool notifyIntentOwner = false);
    }
}