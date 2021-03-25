using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Configuration.Constant;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    [Route("api")]
    [ApiController]
    public class GetTableNameMapController : ControllerBase
    {
        [HttpGet("tables/dynamic/table-name-map")]
        public Dictionary<string, string> Get()
        {
            // map that provides e.g. Select One Flat: SelectOneFlat.

            var availableTables = DynamicTableTypes.GetDynamicTableTypes();
            var tableNameMap = new Dictionary<string, string>();
            foreach (var table in availableTables)
            {
                tableNameMap.Add(table.PrettyName, table.TableType);
            }
            return tableNameMap;
        }
    }
}