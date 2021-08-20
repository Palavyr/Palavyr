#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Services.ConversationServices
{
    public interface IConversationRecordRetriever
    {
        Task<Enquiry[]> RetrieveConversationRecords(string accountId);
        Enquiry MapConvoWithEmailToResponse(ConversationRecord conversationRecord, string accountId);
    }

    public class ConversationRecordRecordRetriever : IConversationRecordRetriever
    {
        private readonly IConvoHistoryRepository convoHistoryRepository;
        private readonly ILogger<IConversationRecordRetriever> logger;

        public ConversationRecordRecordRetriever(
            IConvoHistoryRepository convoHistoryRepository,
            ILogger<IConversationRecordRetriever> logger
        )
        {
            this.convoHistoryRepository = convoHistoryRepository;
            this.logger = logger;
        }

        // Completed means that we've reached the end - the user let all of the messages play out
        // A subset of these will have emails
        public async Task<Enquiry[]> RetrieveConversationRecords(string accountId)
        {
            var conversationRecords = await convoHistoryRepository.GetAllConversationRecords(accountId);


            if (conversationRecords.Count() == 0)
            {
                return new List<Enquiry>().ToArray();
            }

            return FormatEnquiresForDashboard(conversationRecords, accountId);
        }

        private Enquiry[] FormatEnquiresForDashboard(ConversationRecord[] conversationRecords, string accountId)
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