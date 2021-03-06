using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Data;

namespace Palavyr.API.Controllers.Response.DynamicTables.BasicThresholdOps
{
    [Route("api")]
    [ApiController]
    public partial class BasicThresholdController : ControllerBase, IDynamicTableController
    {
        private ILogger<BasicThresholdController> logger;
        private DashContext dashContext;

        public BasicThresholdController(
            DashContext dashContext,
            ILogger<BasicThresholdController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
    }
}