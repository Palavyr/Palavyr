using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.WidgetConfiguration
{

    public class GetDemoWidgetPreCheckController : PalavyrBaseController
    {
        private ILogger<GetDemoWidgetPreCheckController> logger;
        private readonly IConfigurationRepository configurationRepository;
        private readonly WidgetStatusUtils widgetStatusUtils;

        public GetDemoWidgetPreCheckController(
            ILogger<GetDemoWidgetPreCheckController> logger,
            IConfigurationRepository configurationRepository,
            WidgetStatusUtils widgetStatusUtils
        )
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
            this.widgetStatusUtils = widgetStatusUtils;
        }

        [HttpGet("widget-config/demo/pre-check")]
        public async Task<PreCheckResult> Get([FromHeader] string accountId)
        {
            var widgetPrefs = await configurationRepository.GetWidgetPreferences(accountId);
            var areas = await configurationRepository.GetActiveAreasWithConvoAndDynamicAndStaticTables(accountId);

            var result = await widgetStatusUtils.ExecuteWidgetStatusCheck(accountId, areas, widgetPrefs, true, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.PreCheckErrors.Select(x => x.AreaName).ToList()}");
            return result;
        }
    }
}