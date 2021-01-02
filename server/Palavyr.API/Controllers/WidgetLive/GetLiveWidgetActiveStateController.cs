using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Palavyr.API.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.WidgetScheme)]
    [Route("api")]
    [ApiController]
    public class GetLiveWidgetActiveStateController
    {
        private DashContext dashContext;
        private ILogger<GetLiveWidgetActiveStateController> logger;

        public GetLiveWidgetActiveStateController(DashContext dashContext,
            ILogger<GetLiveWidgetActiveStateController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpGet("widget/widget-active-state")]
        public async Task<bool> GetWidgetActiveState([FromHeader] string accountId)
        {
            logger.LogDebug("Retrieving widget state.");
            var widgetPreferences =
                await dashContext.WidgetPreferences.SingleOrDefaultAsync(row => row.AccountId == accountId);
            return widgetPreferences.WidgetState;
        }
    }
}