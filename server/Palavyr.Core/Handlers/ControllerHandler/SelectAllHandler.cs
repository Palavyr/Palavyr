using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SelectAllHandler : INotificationHandler<SelectAllRequest>
    {
        private readonly IEntityStore<ConversationRecord> convoRecordStore;

        public SelectAllHandler(IEntityStore<ConversationRecord> convoRecordStore)
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