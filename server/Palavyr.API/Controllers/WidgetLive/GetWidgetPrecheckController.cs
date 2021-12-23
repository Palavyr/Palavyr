using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class GetWidgetPreCheckController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IWidgetStatusChecker widgetStatusChecker;
        private ILogger<GetWidgetPreCheckController> logger;
        private readonly IHoldAnAccountId accountId;

        public GetWidgetPreCheckController(
            IConfigurationRepository configurationRepository,
            IWidgetStatusChecker widgetStatusChecker,
            ILogger<GetWidgetPreCheckController> logger,
            IHoldAnAccountId accountId
        )
        {
            this.configurationRepository = configurationRepository;
            this.widgetStatusChecker = widgetStatusChecker;
            this.logger = logger;
            this.accountId = accountId;
        }

        [HttpGet("widget/pre-check")]
        public async Task<PreCheckResult> Get([FromQuery] bool demo)
        {
            logger.LogDebug($"Was the demo query param found? {demo}");
            logger.LogDebug("Running live widget pre-check...");
            logger.LogDebug("Checking if account ID exists...");
            if (accountId.AccountId == null)
            {
                return PreCheckResult.CreateApiKeyResult(false);
            }

            var widgetPrefs = await configurationRepository.GetWidgetPreferences();
            var areas = await configurationRepository.GetActiveAreasWithConvoAndDynamicAndStaticTables();

            var result = await widgetStatusChecker.ExecuteWidgetStatusCheck(areas, widgetPrefs, demo, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.PreCheckErrors.Select(x => x.AreaName).ToList()} ");
            return result;
        }
    }
}