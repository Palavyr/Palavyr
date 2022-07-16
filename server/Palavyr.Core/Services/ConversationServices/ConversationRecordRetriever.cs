
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.ConversationServices
{
    public interface IConversationRecordRetriever
    {
        Task<IEnumerable<ConversationHistoryMeta>> RetrieveConversationRecords();
        Task<int> GetActiveEnquiryCount();
    }

    public class ConversationRecordRetriever : IConversationRecordRetriever
    {
        private readonly IEntityStore<ConversationHistoryMeta> convoRecordStore;

        public ConversationRecordRetriever(
            IEntityStore<ConversationHistoryMeta> convoRecordStore
        )
        {
            this.convoRecordStore = convoRecordStore;
        }

        // Completed means that we've reached the end - the user let all of the messages play out
        // A subset of these will have emails
        public async Task<IEnumerable<ConversationHistoryMeta>> RetrieveConversationRecords()
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