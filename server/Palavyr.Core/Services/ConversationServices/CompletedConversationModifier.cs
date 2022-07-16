using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.ConversationServices
{
    public interface ICompletedConversationModifier
    {
        Task MarkAsSeen(List<MarkAsSeenUpdate> updates);
    }

    public class CompletedConversationModifier : ICompletedConversationModifier
    {
        private readonly IEntityStore<ConversationHistoryMeta> recordStore;

        public CompletedConversationModifier(IEntityStore<ConversationHistoryMeta> recordStore, IConversationRecordRetriever conversationRecordRetriever)
        {
            this.recordStore = recordStore;
        }

        public async Task MarkAsSeen(List<MarkAsSeenUpdate> updates)
        {
            var ids = updates.Select(x => x.ConversationId).ToList();
            var records = await recordStore.GetMany(ids, s => s.ConversationId);

            foreach (var update in updates)
            {
                var record = records.SingleOrDefault(x => x.ConversationId == update.ConversationId);
                if (!(record is null))
                {
                    record.Seen = update.Seen;
                }
            }
        }
    }
}