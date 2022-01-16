using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class UnselectAllHandler : INotificationHandler<UnselectAllRequest>
    {
        private readonly IConvoHistoryRepository convoHistoryRepository;

        public UnselectAllHandler(IConvoHistoryRepository convoHistoryRepository)
        {
            this.convoHistoryRepository = convoHistoryRepository;
        }

        public async Task Handle(UnselectAllRequest request, CancellationToken cancellationToken)
        {
            var allRecords = await convoHistoryRepository.GetAllConversationRecords();
            foreach (var conversationRecord in allRecords)
            {
                conversationRecord.Seen = false;
            }

            await convoHistoryRepository.CommitChangesAsync();
        }
    }

    public class UnselectAllRequest : INotification
    {
    }
}