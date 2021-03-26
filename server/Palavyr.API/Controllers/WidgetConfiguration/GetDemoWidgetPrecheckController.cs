using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Common.Utils;
using Palavyr.Domain;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.WidgetConfiguration
{

    public class GetDemoWidgetPreCheckController : PalavyrBaseController
    {
        private ILogger<GetDemoWidgetPreCheckController> logger;
        private readonly IDashConnector dashConnector;

        public GetDemoWidgetPreCheckController(
            ILogger<GetDemoWidgetPreCheckController> logger,
            IDashConnector dashConnector
        )
        {
            this.logger = logger;
            this.dashConnector = dashConnector;
        }

        [HttpGet("widget-config/demo/pre-check")]
        public async Task<PreCheckResult> Get([FromHeader] string accountId)
        {
            var widgetPrefs = await dashConnector.GetWidgetPreferences(accountId);
            var areas = await dashConnector.GetActiveAreasWithConvoAndDynamicAndStaticTables(accountId);

            var result = WidgetStatusUtils.ExecuteWidgetStatusCheck(accountId, areas, widgetPrefs, true, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.IncompleteAreas.ToList()}");
            return result;
        }
    }
}