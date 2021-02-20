using System.Threading.Tasks;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Areas
{
    
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ModifyAreaResponseNameController : ControllerBase
    {

        private readonly IDashConnector dashConnector;
        private readonly ILogger<ModifyAreaResponseNameController> logger;

        public ModifyAreaResponseNameController(
            IDashConnector dashConnector,
            ILogger<ModifyAreaResponseNameController> logger
        )
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpPut("areas/update/name/{areaId}")]
        public async Task<string> Modify(
            [FromHeader] string accountId,
            [FromBody] AreaNameText areaNameText,
            string areaId
        )
        {
            var area = await dashConnector.GetAreaById(accountId, areaId);
            if (areaNameText.AreaName != area.AreaName)
            {
                area.AreaName = areaNameText.AreaName;
                await dashConnector.CommitChangesAsync();
            }
            return areaNameText.AreaName;
        }
    }
}