#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Services.ConversationServices
{
    public interface ICompletedConversationRetriever
    {
        Task<Enquiry[]> RetrieveCompletedConversations(string accountId);
        Enquiry MapConvoWithEmailToResponse(ConversationRecord conversationRecord, string accountId);
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

        // Completed means that we've reached the end - the user let all of the messages play out
        // A subset of these will have emails
        public async Task<Enquiry[]> RetrieveCompletedConversations(string accountId)
        {
            var conversationRecords = await convoContext
                .ConversationRecords
                .Where(row => row.AccountId == accountId)
                .ToListAsync();

            if (conversationRecords.Count() == 0)
            {
                return new List<Enquiry>().ToArray();
            }

            return FormatEnquiresForDashboard(conversationRecords, accountId);
        }

        private Enquiry[] FormatEnquiresForDashboard(List<ConversationRecord> conversationRecords, string accountId)
        {
            var enquiries = new List<Enquiry>();

            foreach (var conversationRecord in conversationRecords)
            {
                try
                {
                    var completeEnquiry = MapConvoWithEmailToResponse(conversationRecord, accountId);
                    enquiries.Add(completeEnquiry);
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"Couldn't find the file: {conversationRecord.ResponsePdfId}");
                    logger.LogDebug($"Message: {ex.Message}");
                }
            }

            return enquiries.ToArray();
        }

        public Enquiry MapConvoWithEmailToResponse(ConversationRecord conversationRecord, string accountId)
        {
            var fileId = conversationRecord.ResponsePdfId;
            FileLinkReference? linkReference;
            if (!string.IsNullOrEmpty(fileId))
            {
                linkReference = FileLinkReference.CreateLink("Response PDF", fileId, fileId + ".pdf");
            }
            else
            {
                linkReference = null;
            }

            var hasResponse = linkReference != null;
            var enquiry = BindToEnquiry(conversationRecord, linkReference, hasResponse);
            return enquiry;
        }

        private static Enquiry BindToEnquiry(ConversationRecord conversationRecord, FileLinkReference? linkReference, bool hasResponse)
        {
            return new Enquiry
            {
                Id = conversationRecord.Id,
                ConversationId = conversationRecord.ConversationId,
                LinkReference = linkReference,
                TimeStamp = conversationRecord.TimeStamp.ToString(),
                AccountId = conversationRecord.AccountId,
                AreaName = conversationRecord.AreaName,
                EmailTemplateUsed = conversationRecord.EmailTemplateUsed,
                Seen = conversationRecord.Seen,
                Name = conversationRecord.Name,
                Email = conversationRecord.Email,
                PhoneNumber = conversationRecord.PhoneNumber,
                HasResponse = hasResponse
            };
        }
    }
}