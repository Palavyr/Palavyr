using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Data;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Contracts;

namespace Palavyr.Services.Repositories
{
    public class GenericDynamicTableRepository<TEntity> : IGenericDynamicTableRepository<TEntity> where TEntity : class, ITable
    {
        private readonly DashContext dashContext;
        private readonly IQueryable<TEntity> readonlyQueryExecutor;
        private readonly DbSet<TEntity> queryExecutor;
        private readonly DbSet<DynamicTableMeta> metaQueryExecutor;

        public GenericDynamicTableRepository(DashContext dashContext)
        {
            this.dashContext = dashContext;
            this.readonlyQueryExecutor = dashContext.Set<TEntity>().AsNoTracking();
            this.queryExecutor = dashContext.Set<TEntity>();
            this.metaQueryExecutor = dashContext.DynamicTableMetas;
        }

        public async Task<List<TEntity>> GetAllRows(string accountId, string areaIdentifier, string tableId)
        {
            return await readonlyQueryExecutor
                .Where(
                    row =>
                        row.AccountId == accountId
                        && row.AreaIdentifier == areaIdentifier
                        && row.TableId == tableId)
                .ToListAsync();
        }

        public async Task<List<TEntity>> GetAllRows(string accountId, string areaIdentifier)
        {
            return await readonlyQueryExecutor
                .Where(
                    row =>
                        row.AccountId == accountId
                        && row.AreaIdentifier == areaIdentifier)
                .ToListAsync();
        }


        public async Task SaveTable(
            string accountId,
            string areaIdentifier,
            string tableId,
            List<TEntity> rowUpdates,
            string tableTag,
            string tableType)
        {
            queryExecutor.RemoveRange(await GetAllRows(accountId, areaIdentifier, tableId));
            await queryExecutor.AddRangeAsync(rowUpdates);

            var meta = await metaQueryExecutor.SingleAsync(row => row.TableId == tableId);

            meta.TableTag = tableTag;
            meta.TableType = tableType;

            await dashContext.SaveChangesAsync(); // Need to make sure this saves changes to both tables.
        }

        public async Task UpdateRows(
            string accountId,
            string areaIdentifier,
            string tableId,
            List<TEntity> rowUpdates
        )
        {
            queryExecutor.RemoveRange(await GetAllRows(accountId, areaIdentifier, tableId));
            await queryExecutor.AddRangeAsync(rowUpdates);
            await dashContext.SaveChangesAsync();
        }

        public async Task DeleteTable(string accountId, string areaIdentifier, string tableId)
        {
            metaQueryExecutor.Remove(await metaQueryExecutor.SingleAsync(row => row.TableId == tableId));
            queryExecutor.RemoveRange(await GetAllRows(accountId, areaIdentifier, tableId));
            await dashContext.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllRowsMatchingDynamicResponseId(string accountId, string dynamicResponseId)
        {
            var row = await readonlyQueryExecutor
                .Where(tableRow => tableRow.AccountId == accountId && dynamicResponseId.EndsWith(tableRow.TableId))
                .ToListAsync();
            return row;
        }

        public async Task<List<ConversationNode>> GetConversationNodeByIds(List<string> ids)
        {
            return await dashContext
                .ConversationNodes
                .Where(row => ids.Contains(row.NodeId))
                .OrderBy(row => row.ResolveOrder)
                .ToListAsync();
        }
    }
}