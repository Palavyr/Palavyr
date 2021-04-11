using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public interface IDynamicTableCommandHandler<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        Task DeleteDynamicTable(DynamicTableRequest request);
        Task<List<TEntity>> GetDynamicTableRows(DynamicTableRequest request);
        TEntity GetDynamicRowTemplate(DynamicTableRequest request);
        Task<List<TEntity>> SaveDynamicTable(DynamicTableRequest request, DynamicTable dynamicTable);
    }

    public class DynamicTableCommandHandler<TEntity> : IDynamicTableCommandHandler<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        private ILogger<SelectOneFlatController> logger;
        private readonly IGenericDynamicTableRepository<TEntity> genericDynamicTableRepository;
        private readonly DynamicTableCompilerRetriever retriever;

        public DynamicTableCommandHandler(
            IGenericDynamicTableRepository<TEntity> genericDynamicTableRepository,
            DynamicTableCompilerRetriever retriever,
            ILogger<SelectOneFlatController> logger
        )
        {
            this.genericDynamicTableRepository = genericDynamicTableRepository;
            this.retriever = retriever;
            this.logger = logger;
        }

        public async Task DeleteDynamicTable(DynamicTableRequest request)
        {
            logger.LogInformation($"Deleting dynamic table: {request.TableId}");
            var (accountId, areaIdentifier, tableId) = request;
            await genericDynamicTableRepository.DeleteTable(accountId, areaIdentifier, tableId);
        }

        public async Task<List<TEntity>> GetDynamicTableRows(DynamicTableRequest request)
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
            return tableRows;
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

            var mappedTableRows = workingEntity.UpdateTable(dynamicTable);
            await genericDynamicTableRepository.SaveTable(
                accountId,
                areaIdentifier,
                tableId,
                mappedTableRows,
                dynamicTable.TableTag,
                typeof(TEntity).Name, 
                async context => await entityCompiler.UpdateConversationNode(context, dynamicTable, tableId)
                );

            return await genericDynamicTableRepository.GetAllRows(accountId, areaIdentifier, tableId);
        }
    }
}