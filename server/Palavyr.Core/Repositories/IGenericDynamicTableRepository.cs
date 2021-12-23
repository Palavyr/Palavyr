using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Repositories
{
    public interface IGenericDynamicTableRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllRows(string areaIdentifier, string tableId);
        
        Task SaveTable(
            string areaIdentifier,
            string tableId,
            List<TEntity> rowUpdates,
            string tableTag,
            string tableType,
            Func<DashContext, Task> updateConversationTable = null
            );

        Task UpdateRows(
            string areaIdentifier,
            string tableId,
            List<TEntity> rowUpdates
        );
        
        Task DeleteTable(string areaIdentifier, string tableId);

        Task<List<TEntity>> GetAllRowsMatchingDynamicResponseId(string dynamicResponseId);
        // Task<List<TEntity>> GetAllRowsMatchingDynamicResponseId(string dynamicResponseId);

        Task<List<ConversationNode>> GetConversationNodeByIds(List<string> ids);

    }
}