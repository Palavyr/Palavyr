using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]

    public class ModifyAreaResponseDisplayTitleController : PalavyrBaseController
    {
        private readonly IDashConnector dashConnector;
        private ILogger<ModifyAreaResponseDisplayTitleController> logger;

        public ModifyAreaResponseDisplayTitleController(
            IDashConnector dashConnector,
            ILogger<ModifyAreaResponseDisplayTitleController> logger
        )
        {
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