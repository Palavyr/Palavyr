using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SelectAllHandler : INotificationHandler<SelectAllRequest>
    {
        private readonly IConvoHistoryRepository convoHistoryRepository;

        public SelectAllHandler(IConvoHistoryRepository convoHistoryRepository)
        {
            this.convoHistoryRepository = convoHistoryRepository;
        }

        public async Task Handle(SelectAllRequest request, CancellationToken cancellationToken)
        {
            var allRecords = await convoHistoryRepository.GetAllConversationRecords();
            foreach (var conversationRecord in allRecords)
            {
                conversationRecord.Seen = true;
            }

            await convoHistoryRepository.CommitChangesAsync();
        }
    }

    public class SelectAllRequest : INotification
    {
    }
}