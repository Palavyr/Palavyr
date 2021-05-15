using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Services.ConversationServices
{
    public interface ICompletedConversationRetriever
    {
        Task<Enquiry[]> RetrieveCompletedConversations(string accountId);
        Enquiry MapEnquiryToResponse(CompletedConversation conversation, string accountId);
    }

    public class CompletedConversationRetriever : ICompletedConversationRetriever
    {
        private readonly ConvoContext convoContext;
        private readonly ILogger<ICompletedConversationRetriever> logger;

        public CompletedConversationRetriever(
            ConvoContext convoContext,
            ILogger<ICompletedConversationRetriever> logger
        )
        {
            this.convoContext = convoContext;
            this.logger = logger;
        }

        public async Task<Enquiry[]> RetrieveCompletedConversations(string accountId)
        {
            var completedConversations = await convoContext
                .CompletedConversations
                .Where(row => row.AccountId == accountId)
                .ToListAsync();

            if (completedConversations.Count() == 0)
            {
                return new List<Enquiry>().ToArray();
            }

            return FormatEnquiresForDashboard(completedConversations, accountId);
        }

        private Enquiry[] FormatEnquiresForDashboard(List<CompletedConversation> completedConversation, string accountId)
        {
            var enquiries = new List<Enquiry>();

            foreach (var completedConvo in completedConversation)
            {
                try
                {
                    var completeEnquiry = MapEnquiryToResponse(completedConvo, accountId);
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

        public Enquiry MapEnquiryToResponse(CompletedConversation conversation, string accountId)
        {
            var fileId = conversation.ResponsePdfId;
            var linkReference = FileLinkReference.CreateLink("Response PDF", fileId, fileId + ".pdf");
            var enquiry = BindToEnquiry(conversation, linkReference);
            return enquiry;
        }

        private static Enquiry BindToEnquiry(CompletedConversation conversation, FileLinkReference linkReference)
        {
            return new Enquiry
            {
                Id = conversation.Id,
                ConversationId = conversation.ConversationId,
                LinkReference = linkReference,
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