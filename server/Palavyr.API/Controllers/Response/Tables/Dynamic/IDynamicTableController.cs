using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public interface IDynamicTableController<TEntity> where TEntity : class, IDynamicTable<TEntity>, new()
    {
        Task<DynamicTableData<TEntity>> GetDynamicTableRows(DynamicTableRequest dynamicTableRequest);

        TEntity GetDynamicRowTemplate(DynamicTableRequest dynamicTableRequest);

        Task<List<TEntity>> SaveDynamicTable(
            DynamicTableRequest dynamicTableRequest,
            [FromBody] DynamicTable dynamicTable);

        Task DeleteDynamicTable(DynamicTableRequest dynamicTableRequest);
    }
}