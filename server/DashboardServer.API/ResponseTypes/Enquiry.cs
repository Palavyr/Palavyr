using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.API.pathUtils;
using DashboardServer.API.ResponseTypes;
using Server.Domain;
using Microsoft.Extensions.Logging;

namespace DashboardServer.API.responseTypes
{
    public class Enquiry
    {
        public int Id { get; set; }
        public string ConversationId { get; set; }
        public FileLink ResponsePdfLink { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AccountId { get; set; }
        public string AreaName { get; set; }
        public string EmailTemplateUsed { get; set; }
        public bool Seen { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }



        private static Enquiry BindToEnquiry(CompletedConversation conversation, FileLink fileLink)
        {
            return new Enquiry()
            {
                Id = conversation.Id,
                ConversationId = conversation.ConversationId,
                ResponsePdfLink = fileLink,
                TimeStamp = conversation.TimeStamp,
                AccountId = conversation.AccountId,
                AreaName = conversation.AreaName,
                EmailTemplateUsed = conversation.EmailTemplateUsed,
                Seen = conversation.Seen,
                Name = conversation.Name,
                Email = conversation.Email,
                PhoneNumber = conversation.PhoneNumber
            };
        }
        
        public static async Task<Enquiry> MapEnquiryToResponse(ILogger _logger, CompletedConversation conversation, string accountId, IAmazonS3 s3Client)
        {
            var fileId = conversation.ResponsePdfId;
            _logger.LogInformation("----------------------------------");
            _logger.LogInformation("1. File ID: " + fileId);
            var preSignedUrl = await UriUtils.CreatePresignedUrlResponseLink(_logger, accountId, fileId, s3Client);

            _logger.LogInformation("5. Got Presigned URL");
            var fileLink = FileLink.CreateLink("Response PDF", preSignedUrl, fileId + ".pdf");
            _logger.LogInformation("6. FileLink: " + fileLink);
            var enquiry = BindToEnquiry(conversation, fileLink);
            return enquiry;
        }
    }
}

