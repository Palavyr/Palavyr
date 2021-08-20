using System.Linq;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;

namespace Palavyr.Core.Repositories.Delete
{
    public class ConvoDeleter : ConvoHistoryRepository, IConvoDeleter
    {
        private readonly ConvoContext convoContext;
        private readonly ILogger<ConvoHistoryRepository> logger;

        public ConvoDeleter(ConvoContext convoContext, ILogger<ConvoHistoryRepository> logger): base(convoContext, logger)
        {
            this.convoContext = convoContext;
            this.logger = logger;
        }

        public void DeleteAccount(string accountId)
        {
            DeleteAllCompletedConversationsByAccount(accountId);
            DeleteAllConversationRecordsByAccount(accountId);
        }
        
        public void DeleteAllConversationRecordsByAccount(string accountId)
        {
            logger.LogCritical($"Deleting conversations records from {accountId}");
            var allConvoRecords = convoContext.ConversationHistories.Where(row => row.AccountId == accountId);
            convoContext.ConversationHistories.RemoveRange(allConvoRecords);
        }

        public void DeleteAllCompletedConversationsByAccount(string accountId)
        {
            logger.LogCritical($"Deleting completed conversations from {accountId}");
            var allCompleted = convoContext.ConversationRecords.Where(row => row.AccountId == accountId);
            convoContext.ConversationRecords.RemoveRange(allCompleted);
        }
    }
}