using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Logging;
using Palavyr.API.responseTypes;
using Palavyr.API.ResponseTypes;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.FileSystem.Aws;
using Palavyr.FileSystem.UniqueIdentifiers;
using Server.Domain.Conversation;

namespace Palavyr.API.Utils
{
    public class EnquiryUtils
    {
        private static Enquiry BindToEnquiry(CompletedConversation conversation, FileLink fileLink)
        {
            return new Enquiry()
            {
                Id = conversation.Id,
                ConversationId = conversation.ConversationId,
                ResponsePdfLink = fileLink,
                TimeStamp = conversation.TimeStamp.ToString(TimeUtils.DateTimeFormat),
                AccountId = conversation.AccountId,
                AreaName = conversation.AreaName,
                EmailTemplateUsed = conversation.EmailTemplateUsed,
                Seen = conversation.Seen,
                Name = conversation.Name,
                Email = conversation.Email,
                PhoneNumber = conversation.PhoneNumber
            };
        }
        
        public static async Task<Enquiry> MapEnquiryToResponse(CompletedConversation conversation, string accountId, IAmazonS3 s3Client, ILogger logger)
        {
            var fileId = conversation.ResponsePdfId;
            var preSignedUrl = await UriUtils.CreatePresignedUrlResponseLink(logger, accountId, fileId, s3Client);
            logger.LogDebug("1. File ID: " + fileId);

            var fileLink = FileLink.CreateLink("Response PDF", preSignedUrl, fileId + ".pdf");
            logger.LogDebug("5. Got Pre-signed URL");

            var enquiry = BindToEnquiry(conversation, fileLink);
            logger.LogDebug("6. FileLink: " + fileLink);
            return enquiry;
        }
    }
}