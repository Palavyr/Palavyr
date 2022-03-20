using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class UnselectAllHandler : INotificationHandler<UnselectAllRequest>
    {
        private readonly IEntityStore<ConversationRecord> convoRecordStore;

        public UnselectAllHandler(IEntityStore<ConversationRecord> convoRecordStore)
        {
            this.convoRecordStore = convoRecordStore;
        }

        public async Task Handle(UnselectAllRequest request, CancellationToken cancellationToken)
        {
            var allRecords = await convoRecordStore.GetAll();
            foreach (var conversationRecord in allRecords)
            {
                conversationRecord.Seen = false;
            }
        }
    }

    public class UnselectAllRequest : INotification
    {
    }
}