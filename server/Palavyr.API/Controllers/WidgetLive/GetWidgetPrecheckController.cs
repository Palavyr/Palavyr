using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.CommonResponseTypes;
using Palavyr.API.Services.AuthenticationServices;
using Palavyr.API.Utils;
using Palavyr.Data.Abstractions;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    [Route("api")]
    [ApiController]
    public class GetWidgetPreCheckController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private ILogger<GetWidgetPreCheckController> logger;

        public GetWidgetPreCheckController(
            IDashConnector dashConnector,
            ILogger<GetWidgetPreCheckController> logger
        )
        {
            this.dashConnector = dashConnector;
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

            var result = await WidgetStatusUtils.ExecuteWidgetStatusCheck(accountId, dashConnector, demo, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.IncompleteAreas.ToList()} ");
            return result;
        }
    }
}