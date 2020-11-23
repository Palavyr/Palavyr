using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class RemoveAreaGroupController : ControllerBase 
    {
        private DashContext dashContext;
        private ILogger<RemoveAreaGroupController> logger;
        public RemoveAreaGroupController(DashContext dashContext, ILogger<RemoveAreaGroupController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }
        
        [HttpDelete("group/{groupId}")]
        public async Task<List<GroupMap>> Delete([FromHeader] string accountId, string groupId)
        {
            dashContext.Groups.Remove(dashContext.Groups.Where(row => row.AccountId == accountId).Single(row => row.GroupId == groupId));
            var areas = dashContext.Areas.Where(row => row.AccountId == accountId && row.GroupId == groupId);
            foreach (var area in areas)
            {
                area.GroupId = null;
            }
            dashContext.Areas.UpdateRange(areas);
            await dashContext.SaveChangesAsync();
            var groups = await dashContext.Groups.Where(row => row.AccountId == accountId).ToListAsync();
            return groups;
        }
    }
}