using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Stores.StoreExtensionMethods
{
    public static class ConversationRecordStoreExtensionMethods
    {
        public static async Task<ConversationHistoryMeta> GetSingleRecord(this IEntityStore<ConversationHistoryMeta> convoRecordStore, string conversationRecordId)
        {
            var record = await convoRecordStore
                .Query()
                .SingleAsync(record => record.ConversationId == conversationRecordId);
            return record;
        }
    }
}