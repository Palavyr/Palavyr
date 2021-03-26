using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.Areas
{
    
    [Authorize]

    public class ModifyAreaResponseNameController : PalavyrBaseController
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