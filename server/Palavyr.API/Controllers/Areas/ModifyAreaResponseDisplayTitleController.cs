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
    public class ModifyAreaResponseDisplayTitleController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<ModifyAreaResponseDisplayTitleController> logger;

        public ModifyAreaResponseDisplayTitleController(
            DashContext dashContext,
            ILogger<ModifyAreaResponseDisplayTitleController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpPut("areas/update/display-title/{areaId}")]
        public async Task<string> Modify(
            [FromHeader] string accountId,
            [FromBody] AreaDisplayTitleText areaDisplayTitleText,
            string areaId
        )
        {
            var curArea = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleAsync(row => row.AreaIdentifier == areaId);

            if (areaDisplayTitleText.AreaDisplayTitle != null)
            {
                curArea.AreaDisplayTitle = areaDisplayTitleText.AreaDisplayTitle;
                await dashContext.SaveChangesAsync();
            }

            return areaDisplayTitleText.AreaDisplayTitle;
        }
    }
}