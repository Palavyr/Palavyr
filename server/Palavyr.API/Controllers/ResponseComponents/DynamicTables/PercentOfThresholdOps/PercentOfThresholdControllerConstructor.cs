using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Response.DynamicTables.PercentOfThresholdOps
{
    [Route("api")]
    [ApiController]
    public partial class PercentOfThresholdController : ControllerBase, IDynamicTableController
    {
        private ILogger<PercentOfThresholdController> logger;
        private DashContext dashContext;

        public PercentOfThresholdController(
            DashContext dashContext,
            ILogger<PercentOfThresholdController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
    }
}