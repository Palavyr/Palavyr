using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Stores
{
    public class PricingStrategyEntityStore<TEntity> : IPricingStrategyEntityStore<TEntity> where TEntity : class, ITable
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly ICancellationTokenTransport cancellationTokenTransport;
        private readonly IQueryable<TEntity> readonlyQueryExecutor;
        private readonly DbSet<TEntity> queryExecutor;
        private readonly DbSet<DynamicTableMeta> metaQueryExecutor;

        public PricingStrategyEntityStore(
            IEntityStore<ConversationNode> convoNodeStore,
            IUnitOfWorkContextProvider contextProvider,
            IAccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport)
        {
            this.convoNodeStore = convoNodeStore;
            this.accountIdTransport = accountIdTransport;
            this.cancellationTokenTransport = cancellationTokenTransport;
            this.readonlyQueryExecutor = contextProvider.ConfigurationContext().Set<TEntity>().AsNoTracking();
            this.queryExecutor = contextProvider.ConfigurationContext().Set<TEntity>();
            this.metaQueryExecutor = contextProvider.ConfigurationContext().DynamicTableMetas;
        }

        public async Task<List<TEntity>> GetAllRows(string areaIdentifier, string tableId)
        {
            return await readonlyQueryExecutor
                .Where(
                    row =>
                        row.AccountId == accountIdTransport.AccountId
                        && row.AreaIdentifier == areaIdentifier
                        && row.TableId == tableId)
                .ToListAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task<List<TEntity>> GetAllRows(string areaIdentifier)
        {
            return await readonlyQueryExecutor
                .Where(
                    row =>
                        row.AccountId == accountIdTransport.AccountId
                        && row.AreaIdentifier == areaIdentifier)
                .ToListAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task SaveTable(
            string areaIdentifier,
            string tableId,
            List<TEntity> rowUpdates,
            string tableTag,
            string tableType,
            Func<Task> updateConversationTable = null
        )
        {
            queryExecutor.RemoveRange(await GetAllRows(areaIdentifier, tableId));
            await queryExecutor.AddRangeAsync(rowUpdates);

            var meta = await metaQueryExecutor.SingleAsync(row => row.TableId == tableId);

            meta.TableTag = tableTag;
            meta.TableType = tableType;

            if (updateConversationTable != null)
            {
                await updateConversationTable();
            }
        }

        public async Task UpdateRows(
            string areaIdentifier,
            string tableId,
            List<TEntity> rowUpdates
        )
        {
            queryExecutor.RemoveRange(await GetAllRows(areaIdentifier, tableId));
            await queryExecutor.AddRangeAsync(rowUpdates);
        }

        public async Task DeleteTable(string areaIdentifier, string tableId)
        {
            var table = await metaQueryExecutor.SingleAsync(row => row.TableId == tableId);
            metaQueryExecutor.Remove(table);

            var allRows = await GetAllRows(areaIdentifier, tableId);
            queryExecutor.RemoveRange(allRows);
        }

        public async Task<List<TEntity>> GetAllRowsMatchingDynamicResponseId(string dynamicTypeId)
        {
            var rows = await readonlyQueryExecutor
                .Where(tableRow => dynamicTypeId.EndsWith(tableRow.TableId)) //&& dynamicResponseId.EndsWith(tableRow.TableId)) // TODO: shhould be dynamicType?
                .ToListAsync();
            return rows;
        }

        public async Task<List<ConversationNode>> GetConversationNodeByIds(List<string> ids)
        {
            return await convoNodeStore
                .Query()
                .Where(row => ids.Contains(row.NodeId))
                .OrderBy(row => row.ResolveOrder)
                .ToListAsync(convoNodeStore.CancellationToken);
        }
    }
}