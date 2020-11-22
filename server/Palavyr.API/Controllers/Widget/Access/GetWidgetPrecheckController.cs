using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.ResponseTypes;

namespace Palavyr.API.Controllers
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
        
        [HttpGet("widget/precheck")]
        public IActionResult Get([FromHeader] string accountId)
        {
            logger.LogDebug("Running live widget pre-check...");
            logger.LogDebug("Checking if account ID exists...");
            if (accountId == null)
            {
                return Ok(PreCheckResult.CreateApiKeyResult(false));
            }

            logger.LogDebug($"Using AccountId: {accountId}");
            var areas = dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .ToList();

            logger.LogDebug("Running live widget conversations pre-check...");
            var result = PreCheckUtils.RunConversationsPreCheck(areas, logger);
            logger.LogDebug(
                $"Pre-check run successful. Result: Isready:{result.IsReady} and incomplete areas: {result.IncompleteAreas.ToList()}");
            return Ok(result);
        }
    }
}