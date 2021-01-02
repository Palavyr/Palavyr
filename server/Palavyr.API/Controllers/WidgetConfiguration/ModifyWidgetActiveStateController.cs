using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    [Route("api")]
    [ApiController]
    public class ModifyWidgetActiveStateController
    {
        private DashContext dashContext;
        private ILogger<GetWidgetPreferencesController> logger;

        public ModifyWidgetActiveStateController(DashContext dashContext,
            ILogger<GetWidgetPreferencesController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPost("widget-config/widget-active-state")]
        public async Task<bool> ModifyWidgetActiveState(
            [FromHeader] string accountId,
            [FromQuery] bool state
        )
        {
            logger.LogDebug("Modifying widget preference");
            var widgetPreferences =
                await dashContext.WidgetPreferences.SingleOrDefaultAsync(row => row.AccountId == accountId);
            widgetPreferences.WidgetState = state;
            await dashContext.SaveChangesAsync();
            return state;
        }
    }
}