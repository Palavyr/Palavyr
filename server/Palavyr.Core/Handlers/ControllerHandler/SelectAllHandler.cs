using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SelectAllHandler : INotificationHandler<SelectAllRequest>
    {
        private readonly IEntityStore<ConversationHistoryMeta> convoRecordStore;

        public SelectAllHandler(IEntityStore<ConversationHistoryMeta> convoRecordStore)
        {
            this.convoRecordStore = convoRecordStore;
        }

        public async Task Handle(SelectAllRequest request, CancellationToken cancellationToken)
        {
            var allRecords = await convoRecordStore.GetAll();
            foreach (var conversationRecord in allRecords)
            {
                conversationRecord.Seen = true;
            }
        }
    }

    public class SelectAllRequest : INotification
    {
    }
}