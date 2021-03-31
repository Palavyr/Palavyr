using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class GetLiveWidgetActiveStateController : PalavyrBaseController
    {
        private IConfigurationRepository configurationRepository;
        private ILogger<GetLiveWidgetActiveStateController> logger;

        public GetLiveWidgetActiveStateController(
            IConfigurationRepository configurationRepository,
            ILogger<GetLiveWidgetActiveStateController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpGet("widget/widget-active-state")]
        public async Task<bool> GetWidgetActiveState([FromHeader] string accountId)
        {
            logger.LogDebug("Retrieving widget state.");
            var widgetPreferences = await configurationRepository.GetWidgetPreferences(accountId);
            return widgetPreferences.WidgetState;
        }
    }
}