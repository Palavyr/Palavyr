using System.Collections.Generic;
using System.Threading.Tasks;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public interface IGenericDynamicTablesRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllRows(string accountId, string areaIdentifier, string tableId);

        Task SaveRows(
            string accountId,
            string areaIdentifier,
            string tableId,
            List<TEntity> rowUpdates,
            string tableTag,
            string tableType);

        Task DeleteTable(string accountId, string areaIdentifier, string tableId);
    }
}