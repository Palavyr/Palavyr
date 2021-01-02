using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.ResponseTypes;
using Palavyr.API.Utils;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    [Route("api")]
    [ApiController]
    public class GetDemoWidgetPreCheckController : ControllerBase
    {
        private ILogger<GetDemoWidgetPreCheckController> logger;
        private DashContext dashContext;

        public GetDemoWidgetPreCheckController(
            ILogger<GetDemoWidgetPreCheckController> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpGet("widget-config/demo/pre-check")]
        public async Task<PreCheckResult> Get([FromHeader] string accountId)
        {
            var result = await WidgetStatusUtils.ExecuteWidgetStatusCheck(accountId, dashContext, true, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.IncompleteAreas.ToList()} ");
            return result;
        }
    }
}