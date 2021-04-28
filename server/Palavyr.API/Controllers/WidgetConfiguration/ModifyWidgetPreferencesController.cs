using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    public class ModifyWidgetPreferencesController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<ModifyWidgetPreferencesController> logger;

        public ModifyWidgetPreferencesController(IConfigurationRepository configurationRepository, ILogger<ModifyWidgetPreferencesController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpPut("widget-config/preferences")]
        public async Task<WidgetPreference> SaveWidgetPreferences(
            [FromHeader]
            string accountId,
            [FromBody]
            WidgetPreference preferences
        )
        {
            var prefs = await configurationRepository.GetWidgetPreferences(accountId);

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

            if (!string.IsNullOrWhiteSpace(preferences.LandingHeader))
            {
                prefs.LandingHeader = preferences.LandingHeader;
            }

            if (!string.IsNullOrWhiteSpace(preferences.ChatHeader))
            {
                prefs.ChatHeader = preferences.ChatHeader;
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

            await configurationRepository.CommitChangesAsync();

            return await configurationRepository.GetWidgetPreferences(accountId);
        }
    }
}