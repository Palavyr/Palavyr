﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Repositories
{
    public class GenericDynamicTableRepository<TEntity> : IGenericDynamicTableRepository<TEntity> where TEntity : class, ITable
    {
        private readonly DashContext dashContext;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly ICancellationTokenTransport cancellationTokenTransport;
        private readonly IQueryable<TEntity> readonlyQueryExecutor;
        private readonly DbSet<TEntity> queryExecutor;
        private readonly DbSet<DynamicTableMeta> metaQueryExecutor;

        public GenericDynamicTableRepository(DashContext dashContext, IAccountIdTransport accountIdTransport, ICancellationTokenTransport cancellationTokenTransport)
        {
            this.dashContext = dashContext;
            this.accountIdTransport = accountIdTransport;
            this.cancellationTokenTransport = cancellationTokenTransport;
            this.readonlyQueryExecutor = dashContext.Set<TEntity>().AsNoTracking();
            this.queryExecutor = dashContext.Set<TEntity>();
            this.metaQueryExecutor = dashContext.DynamicTableMetas;
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
            Func<DashContext, Task> updateConversationTable = null
        )
        {
            queryExecutor.RemoveRange(await GetAllRows(areaIdentifier, tableId));
            await queryExecutor.AddRangeAsync(rowUpdates);

            var meta = await metaQueryExecutor.SingleAsync(row => row.TableId == tableId);

            meta.TableTag = tableTag;
            meta.TableType = tableType;

            if (updateConversationTable != null)
            {
                await updateConversationTable(dashContext);
            }

            await dashContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken); // Need to make sure this saves changes to both tables.
        }

        public async Task UpdateRows(
            string areaIdentifier,
            string tableId,
            List<TEntity> rowUpdates
        )
        {
            queryExecutor.RemoveRange(await GetAllRows(areaIdentifier, tableId));
            await queryExecutor.AddRangeAsync(rowUpdates);
            await dashContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task DeleteTable(string areaIdentifier, string tableId)
        {
            var table = await metaQueryExecutor.SingleAsync(row => row.TableId == tableId);
            metaQueryExecutor.Remove(table);

            var allRows = await GetAllRows(areaIdentifier, tableId);
            queryExecutor.RemoveRange(allRows);
            await dashContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken);
        }

        // public async Task<List<TEntity>> GetAllRowsMatchingDynamicResponseId(string dynamicTypeId)
        // {
        //     var rows = await readonlyQueryExecutor
        //         .Where(tableRow => tableRow.AccountId == accountIdHolder.AccountId && dynamicTypeId.EndsWith(tableRow.TableId)) //&& dynamicResponseId.EndsWith(tableRow.TableId)) // TODO: shhould be dynamicType?
        //         .ToListAsync();
        //     return rows;
        // }

        public async Task<List<TEntity>> GetAllRowsMatchingDynamicResponseId(string dynamicTypeId)
        {
            var rows = await readonlyQueryExecutor
                .Where(tableRow => dynamicTypeId.EndsWith(tableRow.TableId)) //&& dynamicResponseId.EndsWith(tableRow.TableId)) // TODO: shhould be dynamicType?
                .ToListAsync();
            return rows;
        }

        public async Task<List<ConversationNode>> GetConversationNodeByIds(List<string> ids)
        {
            return await dashContext
                .ConversationNodes
                .Where(row => ids.Contains(row.NodeId))
                .OrderBy(row => row.ResolveOrder)
                .ToListAsync(cancellationTokenTransport.CancellationToken);
        }
    }
}