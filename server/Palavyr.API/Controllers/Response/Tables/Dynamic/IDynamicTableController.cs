using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Requests;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public interface IDynamicTableController<TResource> where TResource : IPricingStrategyTableRowResource, new()
    {
        Task DeleteDynamicTable(string intentId, string tableId);
        Task<TResource> GetDynamicRowTemplate([FromRoute] string intentId, [FromRoute] string tableId);
        Task<DynamicTableDataResource<TResource>> GetDynamicTableRows([FromRoute] string intentId, [FromRoute] string tableId);


        Task<IEnumerable<TResource>> SaveDynamicTable(
            [FromRoute] string intentId, [FromRoute] string tableId,
            [FromBody] DynamicTable dynamicTable);

    }
}