using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public interface IDynamicTableController<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetDynamicTableRows(DynamicTableRequest dynamicTableRequest);

        TEntity GetDynamicRowTemplate(DynamicTableRequest dynamicTableRequest);

        Task<List<TEntity>> SaveDynamicTable(
            DynamicTableRequest dynamicTableRequest,
            [FromBody] DynamicTable dynamicTable);

        Task DeleteDynamicTable(DynamicTableRequest dynamicTableRequest);
    }
}