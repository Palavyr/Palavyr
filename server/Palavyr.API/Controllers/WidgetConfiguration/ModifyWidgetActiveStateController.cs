using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.WidgetConfiguration
{

    public class ModifyWidgetActiveStateController : PalavyrBaseController
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