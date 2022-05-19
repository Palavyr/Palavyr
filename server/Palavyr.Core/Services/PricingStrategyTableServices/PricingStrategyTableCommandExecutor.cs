using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Requests;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public class PricingStrategyTableData<TEntity> where TEntity : class, IPricingStrategyTable<TEntity>, new()
    {
        public List<TEntity> TableRows { get; set; }
        public bool IsInUse { get; set; }
    }

    public interface IPricingStrategyTableCommandExecutor<TEntity> where TEntity : class, IPricingStrategyTable<TEntity>, new()
    {
        Task DeleteTable(string intentId, string tableId);
        Task<PricingStrategyTableData<TEntity>> GetTableRows(string intentId, string tableId); // TODO: return new object with 'is in use in palavyr tree'
        TEntity GetRowTemplate(string intentId, string tableId);
        Task<IEnumerable<TEntity>> SaveTable(string intentId, string tableId, PricingStrategyTable<TEntity> pricingStrategyTable);
    }

    public class PricingStrategyTableCommandExecutor<TEntity> : IPricingStrategyTableCommandExecutor<TEntity> where TEntity : class, IPricingStrategyTable<TEntity>, new()
    {
        private ILogger<TEntity> logger;
        private readonly IPricingStrategyEntityStore<TEntity> pricingStrategyEntityStore;
        private readonly IPricingStrategyTableCompilerRetriever retriever;
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IAccountIdTransport accountIdTransport;
        private string AccountId => accountIdTransport.AccountId;

        public PricingStrategyTableCommandExecutor(
            IPricingStrategyEntityStore<TEntity> pricingStrategyEntityStore,
            IPricingStrategyTableCompilerRetriever retriever,
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

        public async Task DeleteTable(string intentId, string tableId)
        {
            logger.LogInformation($"Deleting dynamic table: {tableId}");
            await pricingStrategyEntityStore.DeleteTable(intentId, tableId);
        }

        public async Task<PricingStrategyTableData<TEntity>> GetTableRows(string intentId, string tableId)
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

            return new PricingStrategyTableData<TEntity>
            {
                TableRows = tableRows,
                IsInUse = currentDynamic.Count() > 0
            };
        }

        public TEntity GetRowTemplate(string intentId, string tableId)
        {
            logger.LogInformation($"Getting dynamic table row template: {tableId}");
            return (new TEntity()).CreateTemplate(AccountId, intentId, tableId);
        }

        public async Task<IEnumerable<TEntity>> SaveTable(string intentId, string tableId, PricingStrategyTable<TEntity> pricingStrategyTable)
        {
            var workingEntity = new TEntity();
            workingEntity.EnsureValid();
            var entityCompiler = retriever.RetrieveCompiler<TEntity>();
            // var entityCompiler = retriever.RetrieveCompiler(workingEntity.GetType().Name);

            logger.LogInformation($"Saving dynamic table: {tableId}");

            var validationResult = entityCompiler.ValidatePricingStrategyPreSave(pricingStrategyTable);
            if (!validationResult.IsValid)
            {
                throw new MultiMessageDomainException("Failed to validate the pricing strategy", validationResult.Reasons.ToArray());
            }

            var mappedTableRows = workingEntity.UpdateTable(pricingStrategyTable);
            await pricingStrategyEntityStore.SaveTable(
                intentId,
                tableId,
                mappedTableRows,
                pricingStrategyTable.TableTag!,
                typeof(TEntity).Name, async () => await entityCompiler.UpdateConversationNode(pricingStrategyTable, tableId, intentId)
            );

            return await pricingStrategyEntityStore.GetAllRows(intentId, tableId);
        }
    }
}