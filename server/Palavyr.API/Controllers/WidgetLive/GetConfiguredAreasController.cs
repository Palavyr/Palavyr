using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.WidgetScheme)]
    [Route("api")]
    [ApiController]
    public class GetConfiguredAreasController : ControllerBase
    {
        private ILogger<GetConfiguredAreasController> logger;
        private DashContext dashContext;

        public GetConfiguredAreasController(
            DashContext dashContext,
            ILogger<GetConfiguredAreasController> logger
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpGet("widget/areas")]
        public async Task<IActionResult> Get([FromHeader] string accountId)
        {
            logger.LogDebug("Collecting configured areas for live-widget");
            var areas = dashContext.Areas.Where(row => row.AccountId == accountId).ToList();
            return Ok(areas);
        }
    }
}