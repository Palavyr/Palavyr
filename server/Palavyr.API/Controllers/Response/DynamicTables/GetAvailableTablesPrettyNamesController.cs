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
        public IActionResult Get()
        {
            var availableTables = DynamicTableTypes.GetAvailableTablePrettyNames();
            return Ok(availableTables);
        }
    }
}