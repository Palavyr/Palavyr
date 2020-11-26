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
    public class GetWidgetPrecheckController : ControllerBase
    {
        private ILogger<GetWidgetPrecheckController> logger;
        private DashContext dashContext;

        public GetWidgetPrecheckController(
            DashContext dashContext, 
            ILogger<GetWidgetPrecheckController> logger
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }
        
        [HttpGet("widget/pre-check")]
        public async Task<PreCheckResult> Get([FromHeader] string accountId)
        {
            logger.LogDebug("Running live widget pre-check...");
            logger.LogDebug("Checking if account ID exists...");
            if (accountId == null)
            {
                return PreCheckResult.CreateApiKeyResult(false);
            }
            var result = await WidgetStatusUtils.ExecuteWidgetStatusCheck(accountId, dashContext, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.IncompleteAreas.ToList()} ");
            return result;
        }
    }
}