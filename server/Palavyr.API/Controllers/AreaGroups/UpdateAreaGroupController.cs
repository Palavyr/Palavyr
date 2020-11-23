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
    public class UpdateAreaGroupController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<UpdateAreaGroupController> logger;

        public UpdateAreaGroupController(DashContext dashContext, ILogger<UpdateAreaGroupController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPut("group/area/{areaId}/{groupId}")]
        public async Task<List<GroupMap>> Modify([FromHeader] string accountId, string areaId, string groupId)
        {
            var area = await dashContext.Areas.Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);
            area.GroupId = groupId;
            await dashContext.SaveChangesAsync();
            var groups = await dashContext.Groups.Where(row => row.AccountId == accountId).ToListAsync();
            return groups;
        }
    }
}