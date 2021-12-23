using System.Linq;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Repositories.Delete
{
    public class ConvoDeleter : ConvoHistoryRepository, IConvoDeleter
    {
        private readonly ConvoContext convoContext;
        private readonly ILogger<ConvoHistoryRepository> logger;
        private readonly IHoldAnAccountId accountIdHolder;

        public ConvoDeleter(ConvoContext convoContext, ILogger<ConvoHistoryRepository> logger, IHoldAnAccountId accountIdHolder): base(convoContext, logger, accountIdHolder)
        {
            this.convoContext = convoContext;
            this.logger = logger;
            this.accountIdHolder = accountIdHolder;
        }

        public void DeleteAccount()
        {
            DeleteAllCompletedConversationsByAccount();
            DeleteAllConversationRecordsByAccount();
        }
        
        public void DeleteAllConversationRecordsByAccount()
        {
            logger.LogCritical($"Deleting conversations records from {accountIdHolder.AccountId}");
            var allConvoRecords = convoContext.ConversationHistories.Where(row => row.AccountId == accountIdHolder.AccountId);
            convoContext.ConversationHistories.RemoveRange(allConvoRecords);
        }

        public void DeleteAllCompletedConversationsByAccount()
        {
            logger.LogCritical($"Deleting completed conversations from {accountIdHolder.AccountId}");
            var allCompleted = convoContext.ConversationRecords.Where(row => row.AccountId == accountIdHolder.AccountId);
            convoContext.ConversationRecords.RemoveRange(allCompleted);
        }
    }
}