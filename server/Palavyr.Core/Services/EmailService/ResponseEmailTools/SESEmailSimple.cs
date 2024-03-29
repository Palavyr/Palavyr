﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;

namespace Palavyr.Core.Services.EmailService.ResponseEmailTools
{
    public partial class SesEmail
    {
        private static Body CreatePlainBody(string htmlBody, string textBody)
        {
            return new Body
            {
                Html = new Content
                {
                    Charset = "UTF-8",
                    Data = htmlBody
                },
                Text = new Content
                {
                    Charset = "UTF-8",
                    Data = textBody
                }
            };
        }

        /// <summary>
        /// Send an email without attachments.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        /// <param name="textBody"></param>
        /// <returns>bool - success is true, fail is false</returns>
        public async Task<bool> SendEmail(
            string fromAddress,
            string toAddress,
            string subject,
            string htmlBody,
            string textBody,
            bool notifyIntentOwner = false)
        {
            var toAddy = new List<string>() { toAddress };
            if (notifyIntentOwner)
            {
                toAddy.Add(fromAddress);
            }


            var sendRequest = new SendEmailRequest()
            {
                Source = fromAddress,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { toAddress }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = CreatePlainBody(htmlBody, textBody)
                },
            };

            logger.LogDebug("Trying to send email...");
            try
            {
                var response = await EmailClient.SendEmailAsync(sendRequest);
                logger.LogDebug("SES Email send was successful!");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogDebug("SES Email was not sent");
                logger.LogDebug(ex, "Error: {Message}", ex.Message);
                //TODO: If this errors, then we need to send a response that the email couldn't be sent, and then record the email in the bounceback DB.
                return false;
            }
        }
    }
}