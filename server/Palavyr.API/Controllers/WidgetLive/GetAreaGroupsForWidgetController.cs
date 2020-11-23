using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.WidgetScheme)]
    [Route("api")]
    [ApiController]
    public class GetAreaGroupsForWidgetController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<GetAreaGroupsForWidgetController> logger;

        public GetAreaGroupsForWidgetController(DashContext dashContext, ILogger<GetAreaGroupsForWidgetController> logger)
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpGet("widget/groups")]
        public async Task<IActionResult> FetchGroups([FromHeader] string accountId)
        {
            logger.LogDebug("Retrieving area groups for live-widget.");
            var groups =  dashContext.Groups.Where(row => row.AccountId == accountId).ToList();
            return Ok(groups);
        }
    }
}