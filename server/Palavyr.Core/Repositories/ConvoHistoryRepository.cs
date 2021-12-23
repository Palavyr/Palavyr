using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Repositories
{
    public class ConvoHistoryRepository : IConvoHistoryRepository
    {
        private readonly ConvoContext convoContext;
        private readonly ILogger<ConvoHistoryRepository> logger;
        private readonly IHoldAnAccountId accountIdHolder;

        public ConvoHistoryRepository(ConvoContext convoContext, ILogger<ConvoHistoryRepository> logger, IHoldAnAccountId accountIdHolder)
        {
            this.convoContext = convoContext;
            this.logger = logger;
            this.accountIdHolder = accountIdHolder;
        }

        public async Task CommitChangesAsync(CancellationToken cancellationToken = default) // todo: remove default 
        {
            await convoContext.SaveChangesAsync(cancellationToken);
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
            await Task.CompletedTask;
            var updatedRecord = convoContext.Update(newConversationRecord);
            return updatedRecord.Entity;
        }

        public async Task<ConversationRecord[]> GetAllConversationRecords()
        {
            return await convoContext
                .ConversationRecords
                .Where(row => row.AccountId == accountIdHolder.AccountId)
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