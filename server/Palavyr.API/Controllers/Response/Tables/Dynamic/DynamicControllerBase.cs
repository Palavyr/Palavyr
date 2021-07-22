using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.ModelBinding;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public abstract class DynamicControllerBase<TEntity> : PalavyrBaseController, IDynamicTableController<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        private readonly IDynamicTableCommandHandler<TEntity> handler;

        public DynamicControllerBase(IDynamicTableCommandHandler<TEntity> handler)
        {
            this.handler = handler;
        }

        [HttpDelete("area/{areaId}/table/{tableId}")]
        public async Task DeleteDynamicTable([FromRequest] DynamicTableRequest request)
        {
            await handler.DeleteDynamicTable(request);
        }

        [HttpGet("area/{areaId}/table/{tableId}/template")]
        public TEntity GetDynamicRowTemplate([FromRequest] DynamicTableRequest request)
        {
            return handler.GetDynamicRowTemplate(request);
        }

        [HttpGet("area/{areaId}/table/{tableId}")]
        public async Task<DynamicTableData<TEntity>> GetDynamicTableRows([FromRequest] DynamicTableRequest request)
        {
            return await handler.GetDynamicTableRows(request);
        }

        [HttpPut("area/{areaId}/table/{tableId}")]
        public async Task<List<TEntity>> SaveDynamicTable([FromRequest] DynamicTableRequest request, [FromBody] DynamicTable dynamicTable)
        {
            return await handler.SaveDynamicTable(request, dynamicTable);
        }
    }
} 