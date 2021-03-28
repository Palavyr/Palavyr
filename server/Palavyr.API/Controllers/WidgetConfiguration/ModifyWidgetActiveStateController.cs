using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.WidgetConfiguration
{

    public class ModifyWidgetActiveStateController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetWidgetPreferencesController> logger;

        public ModifyWidgetActiveStateController(
            IConfigurationRepository configurationRepository, 
            ILogger<GetWidgetPreferencesController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpPost("widget-config/widget-active-state")]
        public async Task<bool> ModifyWidgetActiveState(
            [FromHeader] string accountId,
            [FromQuery] bool state
        )
        {
            logger.LogDebug("Modifying widget preference");
            var widgetPrefs = await configurationRepository.GetWidgetPreferences(accountId);
            widgetPrefs.WidgetState = state;
            await configurationRepository.CommitChangesAsync();
            return state;
        }
    }
}