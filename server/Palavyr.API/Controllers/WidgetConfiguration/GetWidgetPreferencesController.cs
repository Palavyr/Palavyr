using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    [Route("api")]
    [ApiController]
    public class GetWidgetPreferencesController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<GetWidgetPreferencesController> logger;

        public GetWidgetPreferencesController(DashContext dashContext, ILogger<GetWidgetPreferencesController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpGet("widget-config/preferences")]
        public async Task<IActionResult> GetWidgetPreferences([FromHeader] string accountId)
        {
            var prefs = await dashContext.WidgetPreferences.SingleOrDefaultAsync(row => row.AccountId == accountId);
            return Ok(prefs);
        }
    }
}