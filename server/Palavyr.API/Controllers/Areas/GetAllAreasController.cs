using System.Collections.Generic;
using System.Threading.Tasks;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class GetAllAreasController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private ILogger<GetAllAreasController> logger;

        public GetAllAreasController(IDashConnector dashConnector, ILogger<GetAllAreasController> logger)
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }
        
        [HttpGet("areas")]
        public async Task<List<Area>> Get([FromHeader] string accountId)
        {
            logger.LogDebug("Return all areas");
            var areas = await dashConnector.GetAllAreasShallow(accountId);
            return areas;
        }
    }
}