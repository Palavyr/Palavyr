using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]

    public class ModifyAreaResponseDisplayTitleController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<ModifyAreaResponseDisplayTitleController> logger;

        public ModifyAreaResponseDisplayTitleController(
            IConfigurationRepository configurationRepository,
            ILogger<ModifyAreaResponseDisplayTitleController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpPut("areas/update/display-title/{areaId}")]
        public async Task<string> Modify(
            [FromHeader] string accountId,
            [FromBody] AreaDisplayTitleText areaDisplayTitleText,
            string areaId
        )
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);
            if (areaDisplayTitleText.AreaDisplayTitle != null)
            {
                area.AreaDisplayTitle = areaDisplayTitleText.AreaDisplayTitle;
                await configurationRepository.CommitChangesAsync();
            }

            return areaDisplayTitleText.AreaDisplayTitle;
        }
    }
}