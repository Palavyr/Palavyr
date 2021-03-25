using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Resources.Requests;

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
        private readonly IGenericDynamicTablesRepository<TEntity> genericDynamicTablesRepository;

        public DynamicTableCommandHandler(
            IGenericDynamicTablesRepository<TEntity> genericDynamicTablesRepository,
            ILogger<SelectOneFlatController> logger
        )
        {
            this.genericDynamicTablesRepository = genericDynamicTablesRepository;
            this.logger = logger;
        }

        public async Task DeleteDynamicTable(DynamicTableRequest request)
        {
            logger.LogInformation($"Deleting dynamic table: {request.TableId}");
            var (accountId, areaIdentifier, tableId) = request;
            await genericDynamicTablesRepository.DeleteTable(accountId, areaIdentifier, tableId);
        }

        public async Task<List<TEntity>> GetDynamicTableRows(DynamicTableRequest request)
        {
            logger.LogInformation($"Getting dynamic table rows: {request.TableId}");
            var (accountId, areaIdentifier, tableId) = request;
            var tableRows = await genericDynamicTablesRepository.GetAllRows(accountId, areaIdentifier, tableId);
            if (tableRows.Count == 0)
            {
                tableRows = new List<TEntity>()
                {
                    (new TEntity()).CreateTemplate(accountId, areaIdentifier, tableId)
                };
            }
            await genericDynamicTablesRepository.UpdateRows(accountId, areaIdentifier, tableId, tableRows);
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
            logger.LogInformation($"Saving dynamic table: {request.TableId}");
            var (accountId, areaIdentifier, tableId) = request;
            var mappedTableRows = (new TEntity()).UpdateTable(dynamicTable);
            await genericDynamicTablesRepository.SaveTable(
                accountId,
                areaIdentifier,
                tableId,
                mappedTableRows,
                dynamicTable.TableTag,
                typeof(TEntity).Name);
            return await genericDynamicTablesRepository.GetAllRows(accountId, areaIdentifier, tableId);
        }
    }
}