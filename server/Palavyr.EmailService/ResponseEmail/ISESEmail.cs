using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailService.ResponseEmail
{
    public interface ISesEmail
    {
        /// <summary>
        /// Send an email without attachments.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        /// <param name="textBody"></param>
        /// <returns>bool - success is true, fail is false</returns>
        Task<bool> SendEmail(string fromAddress, string toAddress, string subject, string htmlBody, string textBody);

        /// <summary>
        /// Send an email WITH attachments
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        /// <param name="textBody"></param>
        /// <param name="filePaths"></param>
        /// <param name="fromAddressLabel"></param>
        /// <param name="toAddressLabel"></param>
        /// <returns>bool - Success is True, Fail is false</returns>
        Task<bool> SendEmailWithAttachments(string fromAddress,
            string toAddress,
            string subject,
            string htmlBody,
            string textBody,
            List<string> filePaths,
            string fromAddressLabel = "",
            string toAddressLabel = "");
    }
}