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
        private readonly IWidgetStatusChecker widgetStatusChecker;

        public GetDemoWidgetPreCheckController(
            ILogger<GetDemoWidgetPreCheckController> logger,
            IConfigurationRepository configurationRepository,
            IWidgetStatusChecker widgetStatusChecker
        )
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
            this.widgetStatusChecker = widgetStatusChecker;
        }

        [HttpGet("widget-config/demo/pre-check")]
        public async Task<PreCheckResult> Get()
        {
            var widgetPrefs = await configurationRepository.GetWidgetPreferences();
            var areas = await configurationRepository.GetActiveAreasWithConvoAndDynamicAndStaticTables();

            var result = await widgetStatusChecker.ExecuteWidgetStatusCheck(areas, widgetPrefs, true, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.PreCheckErrors.Select(x => x.AreaName).ToList()}");
            return result;
        }
    }
}