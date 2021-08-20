using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Repositories
{
    public class ConvoHistoryRepository : IConvoHistoryRepository
    {
        private readonly ConvoContext convoContext;
        private readonly ILogger<ConvoHistoryRepository> logger;

        public ConvoHistoryRepository(ConvoContext convoContext, ILogger<ConvoHistoryRepository> logger)
        {
            this.convoContext = convoContext;
            this.logger = logger;
        }

        public async Task CommitChangesAsync()
        {
            await convoContext.SaveChangesAsync();
        }

        public async Task CreateNewConversationRecord(ConversationRecord newConversationRecord)
        {
            await convoContext.ConversationRecords.AddAsync(newConversationRecord);
        }

        public async Task<ConversationRecord> GetConversationRecordById(string conversationId)
        {
            return await convoContext.ConversationRecords.SingleAsync(x => x.ConversationId == conversationId);
        }

        public async Task<ConversationRecord> UpdateConversationRecord(ConversationRecord newConversationRecord)
        {
            var updatedRecord = convoContext.Update(newConversationRecord);
            return updatedRecord.Entity;
        }

        public async Task<ConversationRecord[]> GetAllConversationRecords(string accountId)
        {
            return await convoContext
                .ConversationRecords
                .Where(row => row.AccountId == accountId)
                .ToArrayAsync();
        }

        public async Task<ConversationHistory[]> GetConversationById(string conversationId)
        {
            return await convoContext
                .ConversationHistories
                .Where(x => x.ConversationId == conversationId)
                .ToArrayAsync();
        }
    }
}