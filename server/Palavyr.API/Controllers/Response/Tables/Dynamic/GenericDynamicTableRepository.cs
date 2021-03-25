using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Data;
using Palavyr.Domain.Contracts;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public class GenericRepository<TEntity> : IGenericDynamicTablesRepository<TEntity> where TEntity : class, ITable
    {
        private readonly DashContext dashContext;
        private readonly IQueryable<TEntity> readonlyQueryExecutor;
        private readonly DbSet<TEntity> queryExecutor;
        private readonly DbSet<Domain.Configuration.Schemas.DynamicTableMeta> metaQueryExecutor;

        public GenericRepository(DashContext dashContext)
        {
            this.dashContext = dashContext;
            this.readonlyQueryExecutor = dashContext.Set<TEntity>().AsNoTracking();
            this.queryExecutor = dashContext.Set<TEntity>();
            this.metaQueryExecutor = dashContext.DynamicTableMetas;
        }

        public async Task<List<TEntity>> GetAllRows(string accountId, string areaIdentifier, string tableId)
        {
            var test = await readonlyQueryExecutor
                .Where(row => row.AccountId == accountId && row.AreaIdentifier == areaIdentifier && row.TableId == tableId).ToListAsync();
            return test;
        }

        public async Task SaveRows(
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

        public async Task DeleteTable(string accountId, string areaIdentifier, string tableId)
        {
            metaQueryExecutor.Remove(await metaQueryExecutor.SingleAsync(row => row.TableId == tableId));
            queryExecutor.RemoveRange(await GetAllRows(accountId, areaIdentifier, tableId));
            await dashContext.SaveChangesAsync();
        }
    }
}