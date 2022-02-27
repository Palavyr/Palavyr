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
        private readonly IAccountIdTransport accountIdTransport;

        public ConvoDeleter(ConvoContext convoContext, ILogger<ConvoHistoryRepository> logger, IAccountIdTransport accountIdTransport): base(convoContext, logger, accountIdTransport)
        {
            this.convoContext = convoContext;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public void DeleteAccount()
        {
            DeleteAllCompletedConversationsByAccount();
            DeleteAllConversationRecordsByAccount();
        }
        
        public void DeleteAllConversationRecordsByAccount()
        {
            logger.LogCritical($"Deleting conversations records from {accountIdTransport.AccountId}");
            var allConvoRecords = convoContext.ConversationHistories.Where(row => row.AccountId == accountIdTransport.AccountId);
            convoContext.ConversationHistories.RemoveRange(allConvoRecords);
        }

        public void DeleteAllCompletedConversationsByAccount()
        {
            logger.LogCritical($"Deleting completed conversations from {accountIdTransport.AccountId}");
            var allCompleted = convoContext.ConversationRecords.Where(row => row.AccountId == accountIdTransport.AccountId);
            convoContext.ConversationRecords.RemoveRange(allCompleted);
        }
    }
}