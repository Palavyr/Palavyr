using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public class PricingStrategyTableData<TEntity> where TEntity
        : class, IPricingStrategyTable<TEntity>, IEntity, ITable, new()
    {
        public List<TEntity> TableRows { get; set; }
        public bool IsInUse { get; set; }
        public string TableTag { get; set; }
    }

    public interface IPricingStrategyTableCommandExecutor<TEntity, TR, TCompiler>
        where TEntity : class, IPricingStrategyTable<TEntity>, IEntity, ITable, new()
        where TCompiler : class, IPricingStrategyTableCompiler
        where TR : class, IPricingStrategyTableRowResource
    {
        Task DeleteTable(string intentId, string tableId);
        Task<PricingStrategyTableData<TEntity>> GetTableRows(string intentId, string tableId); // TODO: return new object with 'is in use in palavyr tree'
        TEntity GetRowTemplate(string intentId, string tableId);
        Task<IEnumerable<TEntity>> SaveTable(string intentId, string tableId, string? tableTag, List<TEntity> pricingStrategyTable);
    }

    public class PricingStrategyTableCommandExecutor<TEntity, TCompiler, TR>
        : IPricingStrategyTableCommandExecutor<TEntity, TR, TCompiler>
        where TEntity : class, IPricingStrategyTable<TEntity>, IEntity, ITable, new()
        where TCompiler : class, IPricingStrategyTableCompiler
        where TR : class, IPricingStrategyTableRowResource
    {
        private ILogger<TEntity> logger;
        private readonly IEntityStore<TEntity> pricingStrategyStore;
        private readonly IEntityStore<PricingStrategyTableMeta> psMetaStore;
        private readonly IPricingStrategyTableCompilerRetriever retriever;
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IAccountIdTransport accountIdTransport;

        private string AccountId => accountIdTransport.AccountId;

        public async Task DeleteTable(string intentId, string tableId)
        {
            logger.LogInformation("Deleting dynamic table: {TableId}", tableId);
            await psMetaStore.Delete(tableId, s => s.TableId);
            await pricingStrategyStore.Delete(tableId, s => s.TableId);
        }

        public PricingStrategyTableCommandExecutor(
            IEntityStore<TEntity> pricingStrategyStore,
            IEntityStore<PricingStrategyTableMeta> psMetaStore,
            IPricingStrategyTableCompilerRetriever retriever,
            IEntityStore<ConversationNode> convoNodeStore,
            IAccountIdTransport accountIdTransport,
            ILogger<TEntity> logger
        )
        {
            this.pricingStrategyStore = pricingStrategyStore;
            this.psMetaStore = psMetaStore;
            this.retriever = retriever;
            this.convoNodeStore = convoNodeStore;
            this.accountIdTransport = accountIdTransport;
            this.logger = logger;
        }

        public async Task<PricingStrategyTableData<TEntity>> GetTableRows(string intentId, string tableId)
        {
            logger.LogInformation("Getting dynamic table rows: {TableId}", tableId);
            var tableRows = await pricingStrategyStore.GetMany(tableId, s => s.TableId);
            if (tableRows.ToList().Count == 0)
            {
                var newEntity = (new TEntity()).CreateTemplate(AccountId, intentId, tableId);
                await pricingStrategyStore.Create(newEntity);
                tableRows = new List<TEntity>()
                {
                    newEntity
                };
            }

            var convoNodes = await convoNodeStore.GetMany(intentId, s => s.AreaIdentifier);

            var currentDynamic = convoNodes.Where(
                x =>
                {
                    if (!x.IsDynamicTableNode) return false;
                    if (x.DynamicType == null) return false;
                    return x.DynamicType.EndsWith(tableId);
                });
            var meta = await psMetaStore.Get(tableId, s => s.TableId);

            return new PricingStrategyTableData<TEntity>
            {
                TableRows = tableRows,
                IsInUse = currentDynamic.Count() > 0,
                TableTag = meta.TableTag
            };
        }

        public TEntity GetRowTemplate(string intentId, string tableId)
        {
            logger.LogInformation("Getting dynamic table row template: {TableId}", tableId);
            return (new TEntity()).CreateTemplate(AccountId, intentId, tableId);
        }

        public async Task<IEnumerable<TEntity>> SaveTable(string intentId, string tableId, string tableTag, List<TEntity> tableUpdate)
        {
            var entityCompiler = retriever.RetrieveCompiler<TCompiler>();
            await entityCompiler.UpdateConversationNode(tableUpdate, tableId, intentId);

            var meta = await psMetaStore.Get(tableId, s => s.TableId);
            meta.TableTag = tableTag;
            meta.TableType = typeof(TEntity).Name;

            // need to delete rows if there are fewer than before
            var currentTableRows = await pricingStrategyStore.GetMany(tableId, s => s.TableId);
            if (tableUpdate.Count < currentTableRows.Count)
            {
                var toKeepIds = tableUpdate.Select(x => x.Id).Where(x => x != null).ToList();
                var thoseToDelete = currentTableRows.Where(r => !toKeepIds.Contains(r.Id)).Select(x => x.Id).Cast<int>();
                await pricingStrategyStore.DeleteMany(thoseToDelete);
            }

            await pricingStrategyStore.CreateOrUpdateMany(tableUpdate);
            return await pricingStrategyStore.GetMany(tableId, s => s.TableId);
        }
    }
}