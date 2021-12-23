﻿using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Repositories
{
    public interface IConvoHistoryRepository
    {
        Task CommitChangesAsync(CancellationToken cancellationToken = default);
        
        Task CreateNewConversationRecord(ConversationRecord newConversationRecord);

        Task<ConversationRecord> GetConversationRecordById(string conversationId);
        Task<ConversationRecord> UpdateConversationRecord(ConversationRecord newConversationRecord);
        Task<ConversationRecord[]> GetAllConversationRecords();
        Task<ConversationHistory[]> GetConversationById(string conversationId);

    }
}