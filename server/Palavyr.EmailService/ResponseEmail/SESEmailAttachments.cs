﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using MimeKit;


namespace EmailService
{
    public partial class SESEmail
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
        private static MimeMessage GetMessage(string fromAddressLabel, string fromAddress, string toAddressLabel, string toAddress, string subject, string htmlBody, string textBody, List<string> filePaths)
        {
            var from  = new MailboxAddress(fromAddressLabel ?? "", fromAddress); // TODO support Labels
            var to = new MailboxAddress(toAddressLabel ?? "", toAddress);
            var body = GetMessageBodyWithAttachments(htmlBody, textBody, filePaths).ToMessageBody();

            var message = new MimeMessage();
            message.From.Add(from);
            message.To.Add(to);
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
        public async Task<bool> SendEmailWithAttachments(string fromAddress,
            string toAddress,
            string subject,
            string htmlBody,
            string textBody,
            List<string> filePaths,
            string fromAddressLabel = "",
            string toAddressLabel = "")
        {

            var message = GetMessage(fromAddressLabel, fromAddress, toAddressLabel, toAddress, subject, htmlBody,
                textBody, filePaths);

            var rawSendRequest = new SendRawEmailRequest()
            {
                RawMessage = new RawMessage(GetMessageStream(message)),
            };
            
            _logger.LogDebug("Trying to send email...");
            try
            {
                await EmailClient.SendRawEmailAsync(rawSendRequest);
                _logger.LogDebug("Email Sent Successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Email (with attachments) was not sent. ");
                _logger.LogDebug("Error: " + ex.Message);
                return false;
            }
        }
    }
}