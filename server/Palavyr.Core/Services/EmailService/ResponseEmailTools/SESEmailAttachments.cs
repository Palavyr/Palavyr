﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Palavyr.Core.Services.EmailService.ResponseEmailTools
{
    public partial class SesEmail
    {
        private static BodyBuilder GetMessageBodyWithAttachments(string htmlBody, string textBody, List<string> filePaths)
        {
            var body = new BodyBuilder()
            {
                HtmlBody = htmlBody,
                TextBody = textBody
            };
            foreach (var filePath in filePaths)
            {
                body.Attachments.Add(filePath);
            }

            return body;
        }

        //https://stackoverflow.com/questions/6743139/send-attachments-with-amazon-ses
        private static MimeMessage GetMessage(
            string fromAddressLabel,
            string fromAddress,
            string toAddressLabel,
            string toAddress,
            string subject,
            string htmlBody,
            string textBody,
            List<string> filePaths,
            bool notifyIntentOwner = false)
        {
            var from = new MailboxAddress(fromAddressLabel ?? "", fromAddress); // TODO support Labels
            var to = new MailboxAddress(toAddressLabel ?? "", toAddress);
            var body = GetMessageBodyWithAttachments(htmlBody, textBody, filePaths).ToMessageBody();

            var message = new MimeMessage();
            message.From.Add(from);
            message.To.Add(to);

            if (notifyIntentOwner)
            {
                message.To.Add(new MailboxAddress(fromAddressLabel ?? "", fromAddress));
            }
            message.Subject = subject;
            message.Body = body;

            return message;
        }

        private static MemoryStream GetMessageStream(MimeMessage message)
        {
            var stream = new MemoryStream();
            message.WriteTo(stream);
            return stream;
        }

        public async Task<bool> SendEmailWithAttachments(
            string fromAddress,
            string toAddress,
            string subject,
            string htmlBody,
            string textBody,
            List<string> filePaths,
            string fromAddressLabel = "",
            string toAddressLabel = "",
            bool notifyIntentOwner = true)
        {
            var message = GetMessage(
                fromAddressLabel,
                fromAddress,
                toAddressLabel,
                toAddress,
                subject,
                htmlBody,
                textBody,
                filePaths,
                notifyIntentOwner);

            var rawSendRequest = new SendRawEmailRequest()
            {
                RawMessage = new RawMessage(GetMessageStream(message)),
            };

            logger.LogDebug("Trying to send email...");

            try
            {
                var response = await EmailClient.SendRawEmailAsync(rawSendRequest);
                logger.LogDebug("Email Sent Successfully");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogDebug("Email (with attachments) was not sent. ");
                logger.LogDebug("Error: " + ex.Message);
                return false;
            }
        }
    }
}