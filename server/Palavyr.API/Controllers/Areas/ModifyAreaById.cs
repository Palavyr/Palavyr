using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ModifyAreaById : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<ModifyAreaById> logger;

        public ModifyAreaById(
            AccountsContext accountContext,
            DashContext dashContext,
            ILogger<ModifyAreaById> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpPut("areas/update/{areaId}")]
        public async Task<NoContentResult> Modify([FromHeader] string accountId, [FromBody] Text text, string areaId)
        {
            var newAreaName = text.AreaName;
            var newAreaDisplayTitle = text.AreaDisplayTitle;

            var curArea = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleAsync(row => row.AreaIdentifier == areaId);

            if (text.AreaName != null)
            {
                curArea.AreaName = newAreaName;
            }

            if (text.AreaDisplayTitle != null)
            {
                curArea.AreaDisplayTitle = newAreaDisplayTitle;
            }

            await dashContext.SaveChangesAsync();
            return NoContent();
        }
    }
}