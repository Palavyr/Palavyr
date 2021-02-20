using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.AreaGroups
{
    [Route("api")]
    [ApiController]
    public class ModifyAreaGroupController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<ModifyAreaGroupController> logger;

        public ModifyAreaGroupController(DashContext dashContext, ILogger<ModifyAreaGroupController> logger)
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