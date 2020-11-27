using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class GetAllAreasController : ControllerBase
    {
        private ILogger<GetAllAreasController> logger;
        private DashContext dashContext;

        public GetAllAreasController(DashContext dashContext, ILogger<GetAllAreasController> logger)
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
        
        [HttpGet("areas")]
        public Area[] Get([FromHeader] string accountId)
        {
            logger.LogDebug("Return all areas");
            var areas = dashContext.Areas.Where(row => row.AccountId == accountId).ToArray();
            return areas;
        }
    }
}