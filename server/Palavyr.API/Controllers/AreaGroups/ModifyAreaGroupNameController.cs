using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.AreaGroups
{
    [Route("api")]
    [ApiController]
    public class ModifyAreaGroupNameController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<ModifyAreaGroupNameController> logger;

        public ModifyAreaGroupNameController(DashContext dashContext, ILogger<ModifyAreaGroupNameController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPut("group/{groupId}")]
        public async Task<List<GroupMap>> Modify([FromHeader] string accountId, string groupId, [FromBody] Text text)
        {
            var currentGroup = await dashContext.Groups.Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.GroupId == groupId);
            currentGroup.GroupName = text.GroupName;

            await dashContext.SaveChangesAsync();
            var groups = await dashContext.Groups.Where(row => row.AccountId == accountId).ToListAsync();
            return groups;
        }
    }
}