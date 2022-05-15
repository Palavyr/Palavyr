using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources.Requests;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.DynamicTableService
{
    public class DynamicTableDataResource<TResource> where TResource : IPricingStrategyTableRowResource
    {
        public List<TResource> TableRows { get; set; }
        public bool IsInUse { get; set; }
    }
    
    public class DynamicTableData<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        public List<TEntity> TableRows { get; set; }
        public bool IsInUse { get; set; }
    }

    public interface IDynamicTableCommandExecutor<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        Task DeleteDynamicTable(string intentId, string tableId);
        Task<DynamicTableData<TEntity>> GetDynamicTableRows(string intentId, string tableId); // TODO: return new object with 'is in use in palavyr tree'
        TEntity GetDynamicRowTemplate(string intentId, string tableId);
        Task<IEnumerable<TEntity>> SaveDynamicTable(string intentId, string tableId, DynamicTable dynamicTable);
    }

    public class DynamicTableCommandExecutor<TEntity> : IDynamicTableCommandExecutor<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        private ILogger<TEntity> logger;
        private readonly IPricingStrategyEntityStore<TEntity> pricingStrategyEntityStore;
        private readonly IDynamicTableCompilerRetriever retriever;
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IAccountIdTransport accountIdTransport;
        private string AccountId => accountIdTransport.AccountId;

        public DynamicTableCommandExecutor(
            IPricingStrategyEntityStore<TEntity> pricingStrategyEntityStore,
            IDynamicTableCompilerRetriever retriever,
            IEntityStore<ConversationNode> convoNodeStore,
            IAccountIdTransport accountIdTransport,
            ILogger<TEntity> logger
        )
        {
            this.pricingStrategyEntityStore = pricingStrategyEntityStore;
            this.retriever = retriever;
            this.convoNodeStore = convoNodeStore;
            this.accountIdTransport = accountIdTransport;
            this.logger = logger;
        }

        public async Task DeleteDynamicTable(string intentId, string tableId)
        {
            logger.LogInformation($"Deleting dynamic table: {tableId}");
            await pricingStrategyEntityStore.DeleteTable(intentId, tableId);
        }

        public async Task<DynamicTableData<TEntity>> GetDynamicTableRows(string intentId, string tableId)
        {
            logger.LogInformation($"Getting dynamic table rows: {tableId}");
            var tableRows = (await pricingStrategyEntityStore.GetAllRows(intentId, tableId)).ToList();
            if (tableRows.ToList().Count == 0)
            {
                tableRows = new List<TEntity>()
                {
                    (new TEntity()).CreateTemplate(AccountId, intentId, tableId)
                };
            }

            await pricingStrategyEntityStore.UpdateRows(intentId, tableId, tableRows);

            var convoNodes = await convoNodeStore.GetMany(intentId, s => s.AreaIdentifier);

            var currentDynamic = convoNodes.Where(
                x =>
                {
                    if (!x.IsDynamicTableNode) return false;
                    if (x.DynamicType == null) return false;
                    if (x.DynamicType != null && x.DynamicType.EndsWith(tableId))
                    {
                        return true;
                    }

                    return false;
                });

            return new DynamicTableData<TEntity>
            {
                TableRows = tableRows,
                IsInUse = currentDynamic.Count() > 0
            };
        }

        public TEntity GetDynamicRowTemplate(string intentId, string tableId)
        {
            logger.LogInformation($"Getting dynamic table row template: {tableId}");
            return (new TEntity()).CreateTemplate(AccountId, intentId, tableId);
        }

        public async Task<IEnumerable<TEntity>> SaveDynamicTable(string intentId, string tableId, DynamicTable dynamicTable)
        {
            var workingEntity = new TEntity();
            workingEntity.EnsureValid();
            var entityCompiler = retriever.RetrieveCompiler(workingEntity.GetType().Name);

            logger.LogInformation($"Saving dynamic table: {tableId}");

            var validationResult = entityCompiler.ValidatePricingStrategyPreSave(dynamicTable);
            if (!validationResult.IsValid)
            {
                throw new MultiMessageDomainException("Failed to validate the pricing strategy", validationResult.Reasons.ToArray());
            }

            var mappedTableRows = workingEntity.UpdateTable(dynamicTable);
            await pricingStrategyEntityStore.SaveTable(
                intentId,
                tableId,
                mappedTableRows,
                dynamicTable.TableTag,
                typeof(TEntity).Name, async () => await entityCompiler.UpdateConversationNode(dynamicTable, tableId, intentId)
            );

            return await pricingStrategyEntityStore.GetAllRows(intentId, tableId);
        }
    }
}