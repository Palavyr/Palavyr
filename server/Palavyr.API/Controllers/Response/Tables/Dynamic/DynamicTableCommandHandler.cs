using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public class DynamicTableData<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        public List<TEntity> TableRows { get; set; }
        public bool IsInUse { get; set; }
    }

    public interface IDynamicTableCommandHandler<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        Task DeleteDynamicTable(DynamicTableRequest request);
        Task<DynamicTableData<TEntity>> GetDynamicTableRows(DynamicTableRequest request); // TODO: return new object with 'is in use in palavyr tree'
        TEntity GetDynamicRowTemplate(DynamicTableRequest request);
        Task<List<TEntity>> SaveDynamicTable(DynamicTableRequest request, DynamicTable dynamicTable);
    }

    public class DynamicTableCommandHandler<TEntity> : IDynamicTableCommandHandler<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        private ILogger<SelectOneFlatController> logger;
        private readonly IGenericDynamicTableRepository<TEntity> genericDynamicTableRepository;
        private readonly DynamicTableCompilerRetriever retriever;
        private readonly IConfigurationRepository configurationRepository;

        public DynamicTableCommandHandler(
            IGenericDynamicTableRepository<TEntity> genericDynamicTableRepository,
            DynamicTableCompilerRetriever retriever,
            IConfigurationRepository configurationRepository,
            ILogger<SelectOneFlatController> logger
        )
        {
            this.genericDynamicTableRepository = genericDynamicTableRepository;
            this.retriever = retriever;
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        public async Task DeleteDynamicTable(DynamicTableRequest request)
        {
            logger.LogInformation($"Deleting dynamic table: {request.TableId}");
            var (accountId, areaIdentifier, tableId) = request;
            await genericDynamicTableRepository.DeleteTable(accountId, areaIdentifier, tableId);
        }

        public async Task<DynamicTableData<TEntity>> GetDynamicTableRows(DynamicTableRequest request)
        {
            logger.LogInformation($"Getting dynamic table rows: {request.TableId}");
            var (accountId, areaIdentifier, tableId) = request;
            var tableRows = await genericDynamicTableRepository.GetAllRows(accountId, areaIdentifier, tableId);
            if (tableRows.Count == 0)
            {
                tableRows = new List<TEntity>()
                {
                    (new TEntity()).CreateTemplate(accountId, areaIdentifier, tableId)
                };
            }

            await genericDynamicTableRepository.UpdateRows(accountId, areaIdentifier, tableId, tableRows);
            var convoNodes = await configurationRepository.GetAreaConversationNodes(accountId, areaIdentifier);
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
            // return tableRows;
        }

        public TEntity GetDynamicRowTemplate(DynamicTableRequest request)
        {
            logger.LogInformation($"Getting dynamic table row template: {request.TableId}");
            var (accountId, areaIdentifier, tableId) = request;
            return (new TEntity()).CreateTemplate(accountId, areaIdentifier, tableId);
        }

        public async Task<List<TEntity>> SaveDynamicTable(DynamicTableRequest request, DynamicTable dynamicTable)
        {
            var workingEntity = new TEntity();
            workingEntity.EnsureValid();
            var entityCompiler = retriever.RetrieveCompiler(workingEntity.GetType().Name);

            logger.LogInformation($"Saving dynamic table: {request.TableId}");
            var (accountId, areaIdentifier, tableId) = request;

            var validationResult = entityCompiler.ValidatePricingStrategyPreSave(dynamicTable);
            if (!validationResult.IsValid) throw new DomainException("Failed to validate the pricing strategy");

            var mappedTableRows = workingEntity.UpdateTable(dynamicTable);
            await genericDynamicTableRepository.SaveTable(
                accountId,
                areaIdentifier,
                tableId,
                mappedTableRows,
                dynamicTable.TableTag,
                typeof(TEntity).Name,
                async context => await entityCompiler.UpdateConversationNode(context, dynamicTable, tableId, areaIdentifier, accountId)
            );

            return await genericDynamicTableRepository.GetAllRows(accountId, areaIdentifier, tableId);
        }
    }
}