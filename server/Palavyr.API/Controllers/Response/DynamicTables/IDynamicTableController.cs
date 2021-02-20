using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Response.DynamicTables
{
    public interface IDynamicTableController
    {
        Task<IActionResult> GetDynamicTableRows([FromHeader] string accountId, [FromRoute] string areaId,
            [FromRoute] string tableId);

        Task<IActionResult> GetDynamicRowTemplate([FromHeader] string accountId, [FromRoute] string areaId,
            [FromRoute] string tableId);

        Task<IActionResult> SaveDynamicTable([FromHeader] string accountId, [FromRoute] string areaId,
            [FromRoute] string tableId,
            [FromBody] DynamicTable dynamicTable);

        Task<IActionResult> DeleteDynamicTable([FromHeader] string accountId, [FromRoute] string areaId,
            [FromRoute] string tableId);
    }
}