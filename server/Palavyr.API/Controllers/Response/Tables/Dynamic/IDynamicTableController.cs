using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.Requests;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public interface IDynamicTableController<TResource> where TResource : IPricingStrategyTableRowResource, new()
    {
        Task DeleteDynamicTable(DynamicTableRequest dynamicTableRequest);
        Task<TResource> GetDynamicRowTemplate(DynamicTableRequest dynamicTableRequest);
        Task<DynamicTableDataResource<TResource>> GetDynamicTableRows(DynamicTableRequest dynamicTableRequest);


        Task<IEnumerable<TResource>> SaveDynamicTable(
            DynamicTableRequest dynamicTableRequest,
            [FromBody] DynamicTable dynamicTable);

    }
}