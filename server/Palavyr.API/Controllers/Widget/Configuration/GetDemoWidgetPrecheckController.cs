using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            logger.LogDebug("Collecting areas for DEMO pre-check...");
            var areas = dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .ToList();

            logger.LogDebug("Collected areas.... running DEMO pre-check");
            var result = PreCheckUtils.RunConversationsPreCheck(areas, logger);
                
            logger.LogDebug($"Pre-check run successful. Result: Isready -- {result.IsReady} and Incomplete areas: {result.IncompleteAreas.ToList()}");
            return Ok(result);
        }
    }
}