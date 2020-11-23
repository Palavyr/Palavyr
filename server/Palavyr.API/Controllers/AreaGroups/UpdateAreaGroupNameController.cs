using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.ReceiverTypes;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class UpdateAreaGroupNameController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<UpdateAreaGroupNameController> logger;

        public UpdateAreaGroupNameController(DashContext dashContext, ILogger<UpdateAreaGroupNameController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPut("group/{groupId}")]
        public async Task<List<GroupMap>> Modify([FromHeader] string accountId, string groupId, [FromBody] Text text)
        {
            var currentGroup = dashContext.Groups.Where(row => row.AccountId == accountId)
                .Single(row => row.GroupId == groupId);
            currentGroup.GroupName = text.GroupName;

            await dashContext.SaveChangesAsync();
            var groups = await dashContext.Groups.Where(row => row.AccountId == accountId).ToListAsync();
            return groups;
        }
    }
}