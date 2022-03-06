using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Repositories.StoreExtensionMethods
{
    public static class IntentConfigurationStoreExtensionMethods
    {
        public static async Task<Area> GetIntentComplete(this IConfigurationEntityStore<Area> intentStore, string intentId)
        {
            var intentComplete = await intentStore
                .Query()
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee)
                .SingleAsync(row => row.AreaIdentifier == intentId, intentStore.CancellationToken);
            return intentComplete;
        }

        public static async Task<Area> GetIntentOnly(this IConfigurationEntityStore<Area> intentStore, string intentId)
        {
            var intentComplete = await intentStore
                .Query()
                .SingleAsync(row => row.AreaIdentifier == intentId,  intentStore.CancellationToken);
            return intentComplete;
        }
    }

    public static class ConversationRecordStoreExtensionMethods
    {
        public static async Task<ConversationRecord> GetSingleRecord(this IConfigurationEntityStore<ConversationRecord> convoRecordStore, string conversationRecordId)
        {
            var record = await convoRecordStore
                .Query()
                .SingleAsync(record => record.ConversationId == conversationRecordId);
            return record;
        }
    }
}