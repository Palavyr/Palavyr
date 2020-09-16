using System.Collections.Generic;
using DashboardServer.API.receiverTypes;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.DynamicTables;

namespace DashboardServer.API.Controllers
{
    public interface IDynamicTableController
    {
        List<SelectOneFlat> GetDynamicTableRows([FromHeader] string accountId, string areaId, string tableId);
        SelectOneFlat GetDynamicRowTemplate([FromHeader] string accountId, string areaId, string tableId);

        List<SelectOneFlat> SaveDynamicTable([FromHeader] string accountId, string areaId, string tableId,
            [FromBody] DynamicTable dynamicTable);

        StatusCodeResult DeleteDynamicTable([FromHeader] string accountId, string areaId, string tableId);
    }
}