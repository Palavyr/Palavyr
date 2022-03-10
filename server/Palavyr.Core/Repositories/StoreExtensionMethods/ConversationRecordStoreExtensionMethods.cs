using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Repositories.StoreExtensionMethods
{
    public static class ConversationRecordStoreExtensionMethods
    {
        public static async Task<ConversationRecord> GetSingleRecord(this IEntityStore<ConversationRecord> convoRecordStore, string conversationRecordId)
        {
            var record = await convoRecordStore
                .Query()
                .SingleAsync(record => record.ConversationId == conversationRecordId);
            return record;
        }
    }
}