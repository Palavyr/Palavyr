using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Response
{
    [Route("api")]
    [ApiController]
    public class GetStaticTablesMetasController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<GetStaticTablesMetasController> logger;

        public GetStaticTablesMetasController(DashContext dashContext, ILogger<GetStaticTablesMetasController> logger)
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
        
        [HttpGet("response/configuration/{areaId}/static/tables")]
        public IActionResult GetStaticTablesMetas([FromHeader] string accountId, string areaId)
        {
            var tables = dashContext
                .StaticTablesMetas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(row => row.StaticTableRows)
                .ThenInclude(x => x.Fee);
            return Ok(tables.ToList());
        }
    }
}