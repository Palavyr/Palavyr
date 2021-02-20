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
    public class DeleteAreaGroupController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<DeleteAreaGroupController> logger;

        public DeleteAreaGroupController(DashContext dashContext, ILogger<DeleteAreaGroupController> logger)
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }
        
        [HttpDelete("group/area/{areaId}")]
        public async Task<List<GroupMap>> Delete([FromHeader] string accountId, string areaId)
        {
            var area = await dashContext.Areas.Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);
            area.GroupId = null;
            await dashContext.SaveChangesAsync();
            var groups = await dashContext.Groups.Where(row => row.AccountId == accountId).ToListAsync();
            return groups;
        }
    }

   
}