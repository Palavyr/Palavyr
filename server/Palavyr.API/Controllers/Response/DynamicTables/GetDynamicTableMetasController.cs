using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.DynamicTables
{
    [Route("api")]
    [ApiController]
    public class GetDynamicTableMetasController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<GetDynamicTableMetasController> logger;

        public GetDynamicTableMetasController(
            DashContext dashContext,
            ILogger<GetDynamicTableMetasController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
        
        /// <summary>
        /// originally used to pul a crazy string from the area table, but now should list off
        /// the current metas from the meta table for a given area
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("tables/dynamic/type/{areaId}")]
        public DynamicTableMeta[] Get([FromHeader] string accountId, string areaId)
        {
            var tableTypes = dashContext
                .DynamicTableMetas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToArray();
            logger.LogDebug("Retrieve Dynamic Table Metas");
            return tableTypes;
        }
    }
}