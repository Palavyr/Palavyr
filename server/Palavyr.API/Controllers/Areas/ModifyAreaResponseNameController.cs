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
    public class ModifyAreaResponseNameController : ControllerBase
    {

        private readonly DashContext dashContext;
        private readonly ILogger<ModifyAreaResponseNameController> logger;

        public ModifyAreaResponseNameController(
            DashContext dashContext,
            ILogger<ModifyAreaResponseNameController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpPut("areas/update/name/{areaId}")]
        public async Task<string> Modify(
            [FromHeader] string accountId,
            [FromBody] AreaNameText areaNameText,
            string areaId
        )
        {
            var curArea = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleAsync(row => row.AreaIdentifier == areaId);

            if (areaNameText.AreaName != curArea.AreaName)
            {
                curArea.AreaName = areaNameText.AreaName;
                await dashContext.SaveChangesAsync();
            }
            return areaNameText.AreaName;
        }
    }
}