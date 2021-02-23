using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Palavyr.Services.AuthenticationServices;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    [Route("api")]
    [ApiController]
    public class GetLiveWidgetActiveStateController
    {
        private IDashConnector dashConnector;
        private ILogger<GetLiveWidgetActiveStateController> logger;

        public GetLiveWidgetActiveStateController(
            IDashConnector dashConnector,
            ILogger<GetLiveWidgetActiveStateController> logger)
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpGet("widget/widget-active-state")]
        public async Task<bool> GetWidgetActiveState([FromHeader] string accountId)
        {
            logger.LogDebug("Retrieving widget state.");
            var widgetPreferences = await dashConnector.GetWidgetPreferences(accountId);
            return widgetPreferences.WidgetState;
        }
    }
}