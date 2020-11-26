using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    [Route("api")]
    [ApiController]
    public class ModifyWidgetPreferencesController : ControllerBase
    {
        private ILogger<ModifyWidgetPreferencesController> logger;
        private DashContext dashContext;

        public ModifyWidgetPreferencesController(DashContext dashContext, ILogger<ModifyWidgetPreferencesController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }
        
        [HttpPut("widget-config/preferences")]
        public async Task<IActionResult> SaveWidgetPreferences([FromHeader] string accountId, [FromBody] WidgetPreference preferences)
        {
            var prefs = await dashContext.WidgetPreferences.SingleOrDefaultAsync(row => row.AccountId == accountId);
            if (prefs == null) return BadRequest();
            if (!string.IsNullOrWhiteSpace(preferences.Title))
            {
                prefs.Title = preferences.Title;
            }

            if (!string.IsNullOrWhiteSpace(preferences.Subtitle))
            {
                prefs.Subtitle = preferences.Subtitle;
            }

            if (!string.IsNullOrWhiteSpace(preferences.Placeholder))
            {
                prefs.Placeholder = preferences.Placeholder;
            }

            if (!string.IsNullOrWhiteSpace(preferences.Header))
            {
                prefs.Header = preferences.Header;
            }

            if (!string.IsNullOrWhiteSpace(preferences.HeaderColor))
            {
                prefs.HeaderColor = preferences.HeaderColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.SelectListColor))
            {
                prefs.SelectListColor = preferences.SelectListColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.FontFamily))
            {
                prefs.FontFamily = preferences.FontFamily;
            }
            
            await dashContext.SaveChangesAsync();
            return NoContent();
        }
    }
}