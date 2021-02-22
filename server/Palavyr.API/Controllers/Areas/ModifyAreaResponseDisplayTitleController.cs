using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;
using Palavyr.Data;
using Palavyr.Data.Abstractions;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ModifyAreaResponseDisplayTitleController : ControllerBase
    {
        private DashContext dashContext;
        private readonly IDashConnector dashConnector;
        private ILogger<ModifyAreaResponseDisplayTitleController> logger;

        public ModifyAreaResponseDisplayTitleController(
            IDashConnector dashConnector,
            ILogger<ModifyAreaResponseDisplayTitleController> logger
        )
        {
            this.dashContext = dashContext;
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpPut("areas/update/display-title/{areaId}")]
        public async Task<string> Modify(
            [FromHeader] string accountId,
            [FromBody] AreaDisplayTitleText areaDisplayTitleText,
            string areaId
        )
        {
            var area = await dashConnector.GetAreaById(accountId, areaId);
            if (areaDisplayTitleText.AreaDisplayTitle != null)
            {
                area.AreaDisplayTitle = areaDisplayTitleText.AreaDisplayTitle;
                await dashConnector.CommitChangesAsync();
            }

            return areaDisplayTitleText.AreaDisplayTitle;
        }
    }
}