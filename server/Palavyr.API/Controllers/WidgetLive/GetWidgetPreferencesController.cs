using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    [Route("api")]
    [ApiController]
    public class GetWidgetPreferencesController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<GetWidgetPreferencesController> logger;

        public GetWidgetPreferencesController(DashContext dashContext, ILogger<GetWidgetPreferencesController> logger)
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpGet("widget/preferences")]
        public async Task<IActionResult> FetchPreferences([FromHeader] string accountId)
        {
            var prefs = await dashContext.WidgetPreferences.SingleOrDefaultAsync(row => row.AccountId == accountId);
            return Ok(prefs);
        }
    }
}