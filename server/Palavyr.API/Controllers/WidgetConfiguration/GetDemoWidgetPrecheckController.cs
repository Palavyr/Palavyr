using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.CommonResponseTypes;
using Palavyr.API.Utils;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    [Route("api")]
    [ApiController]
    public class GetDemoWidgetPreCheckController : ControllerBase
    {
        private ILogger<GetDemoWidgetPreCheckController> logger;
        private readonly IDashConnector dashConnector;
        private DashContext dashContext;

        public GetDemoWidgetPreCheckController(
            ILogger<GetDemoWidgetPreCheckController> logger,
            IDashConnector dashConnector
        )
        {
            this.logger = logger;
            this.dashConnector = dashConnector;
            this.dashContext = dashContext;
        }

        [HttpGet("widget-config/demo/pre-check")]
        public async Task<PreCheckResult> Get([FromHeader] string accountId)
        {
            var result = await WidgetStatusUtils.ExecuteWidgetStatusCheck(accountId, dashConnector, true, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.IncompleteAreas.ToList()}");
            return result;
        }
    }
}