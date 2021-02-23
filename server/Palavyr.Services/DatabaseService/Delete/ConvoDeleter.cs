using System.Linq;
using Microsoft.Extensions.Logging;
using Palavyr.Data;

namespace Palavyr.Services.DatabaseService.Delete
{
    public interface IConvoDeleter : IConvoConnector
    {
        void DeleteAccount(string accountId);
        void DeleteAllConversationRecordsByAccount(string accountId);
        void DeleteAllCompletedConversationsByAccount(string accountId);
    }

    public class ConvoDeleter : ConvoConnector, IConvoDeleter
    {
        private readonly ConvoContext convoContext;
        private readonly ILogger<ConvoConnector> logger;

        public ConvoDeleter(ConvoContext convoContext, ILogger<ConvoConnector> logger): base(convoContext, logger)
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
            var allConvoRecords = convoContext.Conversations.Where(row => row.AccountId == accountId);
            convoContext.Conversations.RemoveRange(allConvoRecords);
        }

        public void DeleteAllCompletedConversationsByAccount(string accountId)
        {
            logger.LogCritical($"Deleting completed conversations from {accountId}");
            var allCompleted = convoContext.CompletedConversations.Where(row => row.AccountId == accountId);
            convoContext.CompletedConversations.RemoveRange(allCompleted);
        }
    }
}