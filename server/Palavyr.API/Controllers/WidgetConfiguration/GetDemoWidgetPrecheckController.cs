using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.WidgetConfiguration
{

    public class GetDemoWidgetPreCheckController : PalavyrBaseController
    {
        private ILogger<GetDemoWidgetPreCheckController> logger;
        private readonly IConfigurationRepository configurationRepository;

        public GetDemoWidgetPreCheckController(
            ILogger<GetDemoWidgetPreCheckController> logger,
            IConfigurationRepository configurationRepository
        )
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
        }

        [HttpGet("widget-config/demo/pre-check")]
        public async Task<PreCheckResult> Get([FromHeader] string accountId)
        {
            var widgetPrefs = await configurationRepository.GetWidgetPreferences(accountId);
            var areas = await configurationRepository.GetActiveAreasWithConvoAndDynamicAndStaticTables(accountId);

            var result = WidgetStatusUtils.ExecuteWidgetStatusCheck(accountId, areas, widgetPrefs, true, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.IncompleteAreas.ToList()}");
            return result;
        }
    }
}