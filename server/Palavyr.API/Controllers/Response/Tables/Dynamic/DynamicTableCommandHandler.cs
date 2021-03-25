using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var (accountId, areaIdentifier, tableId) = request;
            await genericDynamicTablesRepository.DeleteTable(accountId, areaIdentifier, tableId);
        }

        public async Task<List<TEntity>> GetDynamicTableRows(DynamicTableRequest request)
        {
            var (accountId, areaIdentifier, tableId) = request;
            var test = await genericDynamicTablesRepository.GetAllRows(accountId, areaIdentifier, tableId);
            return test;
        }

        public TEntity GetDynamicRowTemplate(DynamicTableRequest request)
        {
            var (accountId, areaIdentifier, tableId) = request;
            return (new TEntity()).CreateTemplate(accountId, areaIdentifier, tableId);
        }

        public async Task<List<TEntity>> SaveDynamicTable(DynamicTableRequest request, DynamicTable dynamicTable)
        {
            var (accountId, areaIdentifier, tableId) = request;
            var mappedTableRows = (new TEntity()).UpdateTable(dynamicTable);
            await genericDynamicTablesRepository.SaveRows(
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