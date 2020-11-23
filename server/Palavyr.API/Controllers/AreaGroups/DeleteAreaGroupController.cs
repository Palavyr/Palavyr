using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.schema;


namespace Palavyr.API.Controllers
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