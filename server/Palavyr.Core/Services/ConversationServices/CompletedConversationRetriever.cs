using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Services.ConversationServices
{
    public class CompletedConversationRetriever
    {
        private readonly IConfiguration configuration;
        private readonly ConvoContext convoContext;
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<CompletedConversationRetriever> logger;

        private string PreviewBucket => configuration.GetSection(ConfigSections.PreviewSection).Value;

        public CompletedConversationRetriever(
            IConfiguration configuration,
            ConvoContext convoContext,
            IAmazonS3 s3Client,
            ILogger<CompletedConversationRetriever> logger
        )
        {
            this.configuration = configuration;
            this.convoContext = convoContext;
            this.s3Client = s3Client;
            this.logger = logger;
        }

        public async Task<Enquiry[]> RetrieveCompletedConversations(string accountId)
        {
            var completedConvos = convoContext
                .CompletedConversations
                .Where(row => row.AccountId == accountId)
                .ToArray();

            if (completedConvos.Count() == 0)
            {
                return new List<Enquiry>().ToArray();
            }

            return await FormatEnquiresForDashboard(completedConvos, accountId);
        }

        private async Task<Enquiry[]> FormatEnquiresForDashboard(CompletedConversation[] completedConversation, string accountId)
        {
            var enquiries = new List<Enquiry>();

            foreach (var completedConvo in completedConversation)
            {
                try
                {
                    var completeEnquiry = await MapEnquiryToResponse(completedConvo, accountId);
                    enquiries.Add(completeEnquiry);
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"Couldn't find the file: {completedConvo.ResponsePdfId}");
                    logger.LogDebug($"Message: {ex.Message}");
                }
            }

            return enquiries.ToArray();
        }
        
        public async Task<Enquiry> MapEnquiryToResponse(CompletedConversation conversation, string accountId)
        {
            var fileId = conversation.ResponsePdfId;
            var preSignedUrl = await UriUtils.CreatePreSignedUrlResponseLink(logger, accountId, fileId, s3Client, PreviewBucket);
            var fileLink = FileLink.CreateLink("Response PDF", preSignedUrl, fileId + ".pdf");
            var enquiry = BindToEnquiry(conversation, fileLink);
            return enquiry;
        }

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
    }
}