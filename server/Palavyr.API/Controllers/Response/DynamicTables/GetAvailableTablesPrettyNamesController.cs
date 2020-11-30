using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Configuration.Constant;

namespace Palavyr.API.Controllers.Response.DynamicTables
{
    [Route("api")]
    [ApiController]
    public class GetAvailableTablesController : ControllerBase
    {
        public GetAvailableTablesController()
        { }
        
        [HttpGet("tables/dynamic/available-tables-pretty-names")]
        public string[] Get()
        {
            // var availableTables = DynamicTableTypes.GetAvailableTablePrettyNames();
            var availableTables = DynamicTableTypes
                .GetDynamicTableTypes()
                .Select(x => x.TableType)
                .ToArray();
            return availableTables;
        }
    }
}