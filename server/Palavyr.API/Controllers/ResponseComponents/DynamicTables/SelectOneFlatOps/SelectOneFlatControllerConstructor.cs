using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Response.DynamicTables.SelectOneFlatOps
{
    [Route("api")]
    [ApiController]
    public partial class SelectOneFlatController : ControllerBase, IDynamicTableController
    {
        private ILogger<SelectOneFlatController> logger;
        private DashContext dashContext;
        public SelectOneFlatController(
            DashContext dashContext,
            ILogger<SelectOneFlatController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
    }
}