using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.ModelBinding;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources.Requests;
using Palavyr.Core.Services.DynamicTableService;
// you need to write 6 more mappers at the moment. can you change the api so that you dont have to use one of the mapers?
namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    [ApiController]
    public abstract class DynamicControllerBase<TEntity, TResource>
        : PalavyrBaseController, IDynamicTableController<TResource> where TEntity : class, IDynamicTable<TEntity>, new() where TResource : IPricingStrategyTableRowResource, new()
    {
        private readonly IDynamicTableCommandExecutor<TEntity> executor;
        private readonly IMapToNew<TEntity, TResource> entityMapper;
        private readonly IMapToNew<DynamicTableData<TEntity>, DynamicTableDataResource<TResource>> tableDataMapper;

        public DynamicControllerBase(
            IDynamicTableCommandExecutor<TEntity> executor,
            IMapToNew<TEntity, TResource> entityMapper,
            IMapToNew<DynamicTableData<TEntity>, DynamicTableDataResource<TResource>> tableDataMapper)
        {
            this.executor = executor;
            this.entityMapper = entityMapper;
            this.tableDataMapper = tableDataMapper;
        }

        [HttpDelete("intent/{intentId}/table/{tableId}")]
        public async Task DeleteDynamicTable([FromRoute] string intentId, [FromRoute] string tableId)
        {
            await executor.DeleteDynamicTable(intentId, tableId);
        }

        [HttpGet("intent/{intentId}/table/{tableId}/template")]
        public async Task<TResource> GetDynamicRowTemplate([FromRoute] string intentId, [FromRoute] string tableId)
        {
            var template = executor.GetDynamicRowTemplate(intentId, tableId);
            var resource = await entityMapper.Map(template);
            return resource;
        }

        [HttpGet("intent/{intentId}/table/{tableId}")]
        public async Task<DynamicTableDataResource<TResource>> GetDynamicTableRows([FromRoute] string intentId, [FromRoute] string tableId)
        {
            var rows = await executor.GetDynamicTableRows(intentId, tableId);
            var resource = await tableDataMapper.Map(rows);
            return resource;
        }

        [HttpPut("intent/{intentId}/table/{tableId}")]
        public async Task<IEnumerable<TResource>> SaveDynamicTable([FromRoute] string intentId, [FromRoute] string tableId, [FromBody] DynamicTable dynamicTable)
        {
            var rows = await executor.SaveDynamicTable(intentId, tableId, dynamicTable);
            var resource = await entityMapper.MapMany(rows);
            return resource;
        }
    }
}