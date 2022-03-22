#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.ConversationServices
{
    public interface IConversationRecordRetriever
    {
        Task<IEnumerable<Enquiry>> RetrieveConversationRecords();
    }

    public class ConversationRecordRetriever : IConversationRecordRetriever
    {
        private readonly IEntityStore<ConversationRecord> convoRecordStore;
        private readonly IMapToNew<ConversationRecord, Enquiry> mapper;

        public ConversationRecordRetriever(
            IEntityStore<ConversationRecord> convoRecordStore,
            IMapToNew<ConversationRecord, Enquiry> mapper
        )
        {
            this.convoRecordStore = convoRecordStore;
            this.mapper = mapper;
        }

        // Completed means that we've reached the end - the user let all of the messages play out
        // A subset of these will have emails
        public async Task<IEnumerable<Enquiry>> RetrieveConversationRecords()
        {
            var conversationRecords = await convoRecordStore.GetAll();
            if (conversationRecords.Count() == 0)
            {
                return new List<Enquiry>().ToArray();
            }

            var enquiries = await mapper.MapMany(conversationRecords);
            return enquiries;
        }
    }
}