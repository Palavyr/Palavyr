using System.Collections.Generic;
using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.AreaGroups
{
    [Route("api")]
    [ApiController]
    public class GetAreaGroupsController : ControllerBase 
    {
        private DashContext dashContext;
        private ILogger<GetAreaGroupsController> logger;
        public GetAreaGroupsController(DashContext dashContext, ILogger<GetAreaGroupsController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }
        [HttpGet("group")]
        public List<GroupMap> Get([FromHeader] string accountId)
        {
            var groups = dashContext.Groups.Where(row => row.AccountId == accountId).ToList();
            return groups;
        }
    }
}