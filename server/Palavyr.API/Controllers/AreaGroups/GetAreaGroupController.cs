using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.AreaGroups
{
    [Route("api")]
    [ApiController]
    public class GetAreaGroupController : ControllerBase 
    {
        private DashContext dashContext;
        private ILogger<GetAreaGroupController> logger;
        public GetAreaGroupController(DashContext dashContext, ILogger<GetAreaGroupController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }
        
        [HttpGet("group/{groupId}")]
        public async Task<GroupMap> Get([FromHeader] string accountId, string groupId)
        {
            var group = await dashContext.Groups.Where(row => row.AccountId == accountId).SingleOrDefaultAsync(row => row.GroupId == groupId);
            return group;
        }
        
    }
}