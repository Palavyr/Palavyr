using System.Collections.Generic;
using System.Threading.Tasks;

namespace Palavyr.Services.Repositories
{
    public interface IGenericDynamicTableRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllRows(string accountId, string areaIdentifier, string tableId);
        Task<List<TEntity>> GetAllRows(string accountId, string areaIdentifier);
        
        Task SaveTable(
            string accountId,
            string areaIdentifier,
            string tableId,
            List<TEntity> rowUpdates,
            string tableTag,
            string tableType);

        Task UpdateRows(
            string accountId,
            string areaIdentifier,
            string tableId,
            List<TEntity> rowUpdates
        );
        
        Task DeleteTable(string accountId, string areaIdentifier, string tableId);

        Task<List<TEntity>> GetAllRowsMatchingDynamicResponseId(string accountId, string dynamicResponseId);

    }
}