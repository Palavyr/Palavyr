
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.ConversationServices
{
    public interface IConversationRecordRetriever
    {
        Task<IEnumerable<ConversationRecord>> RetrieveConversationRecords();
        Task<int> GetActiveEnquiryCount();
    }

    public class ConversationRecordRetriever : IConversationRecordRetriever
    {
        private readonly IEntityStore<ConversationRecord> convoRecordStore;

        public ConversationRecordRetriever(
            IEntityStore<ConversationRecord> convoRecordStore
        )
        {
            this.convoRecordStore = convoRecordStore;
        }

        // Completed means that we've reached the end - the user let all of the messages play out
        // A subset of these will have emails
        public async Task<IEnumerable<ConversationRecord>> RetrieveConversationRecords()
        {
            var conversationRecords = await convoRecordStore.GetAll();
            return conversationRecords;
        }


        public async Task<int> GetActiveEnquiryCount()
        {
            var conversationRecords = await convoRecordStore.GetAll();
            var unseen = conversationRecords.Where(x => !x.Seen).ToArray();
            return unseen.Length;
        }
    }
}