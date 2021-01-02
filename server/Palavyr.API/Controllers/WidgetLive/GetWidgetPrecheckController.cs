using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.ResponseTypes;
using Palavyr.API.Services.AuthenticationServices;
using Palavyr.API.Utils;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.WidgetScheme)]
    [Route("api")]
    [ApiController]
    public class GetWidgetPreCheckController : ControllerBase
    {
        private ILogger<GetWidgetPreCheckController> logger;
        private DashContext dashContext;

        public GetWidgetPreCheckController(
            DashContext dashContext,
            ILogger<GetWidgetPreCheckController> logger
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
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

            var result = await WidgetStatusUtils.ExecuteWidgetStatusCheck(accountId, dashContext, demo, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.IncompleteAreas.ToList()} ");
            return result;
        }
    }
}