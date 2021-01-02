using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    [Route("api")]
    [ApiController]
    public class GetWidgetActiveStateController
    {
        
        private DashContext dashContext;
        private ILogger<GetWidgetPreferencesController> logger;

        public GetWidgetActiveStateController(DashContext dashContext, ILogger<GetWidgetPreferencesController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }
        
        [HttpGet("widget-config/widget-active-state")]
        public async Task<bool> GetWidgetActiveState([FromHeader] string accountId)
        {
            logger.LogDebug("Retrieving widget state.");
            var widgetPreferences = await dashContext.WidgetPreferences.SingleOrDefaultAsync(row => row.AccountId == accountId);
            return widgetPreferences.WidgetState;
        }
    }
}