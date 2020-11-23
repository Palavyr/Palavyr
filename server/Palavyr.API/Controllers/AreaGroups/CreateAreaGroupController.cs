using System;
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
    public class CreateAreaGroupController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<CreateAreaGroupController> logger;

        public CreateAreaGroupController(DashContext dashContext, ILogger<CreateAreaGroupController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPost("group")]
        public async Task<List<GroupMap>> Create([FromHeader] string accountId, [FromBody] Text text)
        {
            var groupId = Guid.NewGuid().ToString();
            var newGroup = GroupMap.CreateGroupMap(groupId, text.ParentGroup, text.GroupName, accountId);

            await dashContext.Groups.AddAsync(newGroup);
            await dashContext.SaveChangesAsync();

            var groups = await dashContext.Groups.Where(row => row.AccountId == accountId).ToListAsync();
            return groups;
        }
    }
}