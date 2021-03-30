using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.Areas
{
    
    [Authorize]

    public class ModifyAreaResponseNameController : PalavyrBaseController
    {

        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<ModifyAreaResponseNameController> logger;

        public ModifyAreaResponseNameController(
            IConfigurationRepository configurationRepository,
            ILogger<ModifyAreaResponseNameController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpPut("areas/update/name/{areaId}")]
        public async Task<string> Modify(
            [FromHeader] string accountId,
            [FromBody] AreaNameText areaNameText,
            string areaId
        )
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);
            if (areaNameText.AreaName != area.AreaName)
            {
                area.AreaName = areaNameText.AreaName;
                await configurationRepository.CommitChangesAsync();
            }
            return areaNameText.AreaName;
        }
    }
}