using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.ModelBinding;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public abstract class DynamicControllerBase<TEntity> : PalavyrBaseController, IDynamicTableController<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        private readonly IDynamicTableCommandExecutor<TEntity> executor;

        public DynamicControllerBase(IDynamicTableCommandExecutor<TEntity> executor)
        {
            this.executor = executor;
        }

        [HttpDelete("area/{areaId}/table/{tableId}")]
        public async Task DeleteDynamicTable([FromRequest] DynamicTableRequest request)
        {
            await executor.DeleteDynamicTable(request);
        }

        [HttpGet("area/{areaId}/table/{tableId}/template")]
        public TEntity GetDynamicRowTemplate([FromRequest] DynamicTableRequest request)
        {
            return executor.GetDynamicRowTemplate(request);
        }

        [HttpGet("area/{areaId}/table/{tableId}")]
        public async Task<DynamicTableData<TEntity>> GetDynamicTableRows([FromRequest] DynamicTableRequest request)
        {
            return await executor.GetDynamicTableRows(request);
        }

        [HttpPut("area/{areaId}/table/{tableId}")]
        public async Task<List<TEntity>> SaveDynamicTable([FromRequest] DynamicTableRequest request, [FromBody] DynamicTable dynamicTable)
        {
            return await executor.SaveDynamicTable(request, dynamicTable);
        }
    }
} 