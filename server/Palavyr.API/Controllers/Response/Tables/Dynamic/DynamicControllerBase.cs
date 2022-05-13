using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.ModelBinding;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.Requests;
using Palavyr.Core.Services.DynamicTableService;
you need to write 6 more mappers at the moment. can you change the api so that you dont have to use one of the mapers?
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

        [HttpDelete("area/{areaId}/table/{tableId}")]
        public async Task DeleteDynamicTable([FromRequest] DynamicTableRequest request)
        {
            await executor.DeleteDynamicTable(request);
        }

        [HttpGet("area/{areaId}/table/{tableId}/template")]
        public async Task<TResource> GetDynamicRowTemplate([FromRequest] DynamicTableRequest request)
        {
            var template = executor.GetDynamicRowTemplate(request);
            var resource = await entityMapper.Map(template);
            return resource;
        }

        [HttpGet("area/{areaId}/table/{tableId}")]
        public async Task<DynamicTableDataResource<TResource>> GetDynamicTableRows([FromRequest] DynamicTableRequest request)
        {
            var rows = await executor.GetDynamicTableRows(request);
            var resource = await tableDataMapper.Map(rows);
            return resource;
        }

        [HttpPut("area/{areaId}/table/{tableId}")]
        public async Task<IEnumerable<TResource>> SaveDynamicTable([FromRequest] DynamicTableRequest request, [FromBody] DynamicTable dynamicTable)
        {
            var rows = await executor.SaveDynamicTable(request, dynamicTable);
            var resource = await entityMapper.MapMany(rows);
            return resource;
        }
    }
}