using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;

namespace Palavyr.API.controllers.widget
{

    [Route("api")]
    [ApiController]
    public class GetDemoWidgetPrecheckController : ControllerBase
    {
        private ILogger<GetDemoWidgetPrecheckController> logger;
        private DashContext dashContext;

        public GetDemoWidgetPrecheckController(
            ILogger<GetDemoWidgetPrecheckController> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }
    
        [HttpGet("widget-config/demo/pre-check")]
        public async Task<IActionResult> Get([FromHeader] string accountId)
        {
            var result = WidgetStatusUtils.ExecuteWidgetStatusCheck(accountId, dashContext, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.IncompleteAreas.ToList()} ");
            return Ok(result);
        }
    }
}