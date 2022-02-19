using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.Environment;

namespace Palavyr.Core.Services.EmailService.SmtpEmail
{
    public interface ISmtpEmailClient
    {
        Task SendSmtpEmail(string fromAddress, string toAddress, string subject, string htmlBody, string textBody = "");

        Task SendSmtpEmailWithAttachments(
            string fromAddress,
            string toAddress,
            string subject,
            string htmlBody,
            List<string> filePaths);
    }

    public class SmtpEmailClient : ISmtpEmailClient, IDisposable
    {
        private readonly IConfiguration configuration;
        private readonly IDetermineCurrentOperatingSystem determineCurrentOperatingSystem;
        private readonly string SMTP_USERNAME;
        private readonly string SMTP_PASSWORD;
        private string HOST;
        private int PORT;

        private readonly SmtpClient smtpClient;

        public SmtpEmailClient(IConfiguration configuration, IDetermineCurrentOperatingSystem determineCurrentOperatingSystem)
        {
            this.configuration = configuration;
            this.determineCurrentOperatingSystem = determineCurrentOperatingSystem;
            // SMTP_USERNAME = "";//configuration.GetSmtpUsername();
            // SMTP_PASSWORD = "";//configuration.GetSmtpPassword();

            HOST = "email-smtp.us-east-1.amazonaws.com";
            PORT = 587;

            smtpClient = new SmtpClient(HOST, PORT);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            smtpClient.EnableSsl = true;
        }

        private MailMessage CreateMessage(string fromAddress, string toAddress, string subject, string htmlBody)
        {
            var message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(fromAddress, ""); // TODO get a display name maybe?
            message.To.Add(new MailAddress(toAddress));
            message.Subject = subject;
            message.Body = htmlBody;
            return message;
        }

        public async Task SendSmtpEmail(string fromAddress, string toAddress, string subject, string htmlBody, string textBody = "")
        {
            var message = CreateMessage(fromAddress, toAddress, subject, htmlBody);
            await smtpClient.SendMailAsync(message);
            Console.WriteLine("Email sent!");
        }

        public async Task SendSmtpEmailWithAttachments(
            string fromAddress,
            string toAddress,
            string subject,
            string htmlBody,
            List<string> filePaths)
        {
            var message = CreateMessage(fromAddress, toAddress, subject, htmlBody);
            foreach (var filePath in filePaths)
            {
                var contentType = new System.Net.Mime.ContentType();
                contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                message.Attachments.Add(new Attachment(filePath, contentType));
            }

            await smtpClient.SendMailAsync(message);
            Console.WriteLine("Email sent!");
        }

        public void Dispose()
        {
            smtpClient?.Dispose();
        }
    }
}