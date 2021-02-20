using System.Threading.Tasks;
using DashboardServer.Data;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    [Route("api")]
    [ApiController]
    public class ModifyWidgetActiveStateController
    {
        private readonly IDashConnector dashConnector;
        private ILogger<GetWidgetPreferencesController> logger;

        public ModifyWidgetActiveStateController(
            IDashConnector dashConnector, 
            ILogger<GetWidgetPreferencesController> logger)
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpPost("widget-config/widget-active-state")]
        public async Task<bool> ModifyWidgetActiveState(
            [FromHeader] string accountId,
            [FromQuery] bool state
        )
        {
            logger.LogDebug("Modifying widget preference");
            var widgetPrefs = await dashConnector.GetWidgetPreferences(accountId);
            widgetPrefs.WidgetState = state;
            await dashConnector.CommitChangesAsync();
            return state;
        }
    }
}