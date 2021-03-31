using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class GetWidgetPreCheckController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetWidgetPreCheckController> logger;

        public GetWidgetPreCheckController(
            IConfigurationRepository configurationRepository,
            ILogger<GetWidgetPreCheckController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpGet("widget/pre-check")]
        public async Task<PreCheckResult> Get([FromHeader] string accountId, [FromQuery] bool demo)
        { 
            logger.LogDebug($"Was the demo query param found? {demo}");
            logger.LogDebug("Running live widget pre-check...");
            logger.LogDebug("Checking if account ID exists...");
            if (accountId == null)
            {
                return PreCheckResult.CreateApiKeyResult(false);
            }

            var widgetPrefs = await configurationRepository.GetWidgetPreferences(accountId);
            var areas = await configurationRepository.GetActiveAreasWithConvoAndDynamicAndStaticTables(accountId);

            var result = WidgetStatusUtils.ExecuteWidgetStatusCheck(accountId, areas, widgetPrefs, demo, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.IncompleteAreas.ToList()} ");
            return result;
        }
    }
}