using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Data.Abstractions;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    [Route("api")]
    [ApiController]
    public class ModifyWidgetPreferencesController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private ILogger<ModifyWidgetPreferencesController> logger;

        public ModifyWidgetPreferencesController(IDashConnector dashConnector, ILogger<ModifyWidgetPreferencesController> logger)
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpPut("widget-config/preferences")]
        public async Task SaveWidgetPreferences(
            [FromHeader] string accountId,
            [FromBody] WidgetPreference preferences
        )
        {
            var prefs = await dashConnector.GetWidgetPreferences(accountId);

            if (!string.IsNullOrWhiteSpace(preferences.SelectListColor))
            {
                prefs.SelectListColor = preferences.SelectListColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.HeaderColor))
            {
                prefs.HeaderColor = preferences.HeaderColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.FontFamily))
            {
                prefs.FontFamily = preferences.FontFamily;
            }

            if (!string.IsNullOrWhiteSpace(preferences.Header))
            {
                prefs.Header = preferences.Header;
            }

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

            if (!string.IsNullOrWhiteSpace(preferences.ListFontColor))
            {
                prefs.ListFontColor = preferences.ListFontColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.HeaderFontColor))
            {
                prefs.HeaderFontColor = preferences.HeaderFontColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.OptionsHeaderColor))
            {
                prefs.OptionsHeaderColor = preferences.OptionsHeaderColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.OptionsHeaderFontColor))
            {
                prefs.OptionsHeaderFontColor = preferences.OptionsHeaderFontColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.ChatFontColor))
            {
                prefs.ChatFontColor = preferences.ChatFontColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.ChatBubbleColor))
            {
                prefs.ChatBubbleColor = preferences.ChatBubbleColor;
            }

            await dashConnector.CommitChangesAsync();
        }
    }
}