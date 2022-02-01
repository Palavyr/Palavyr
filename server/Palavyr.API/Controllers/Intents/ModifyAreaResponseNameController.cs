using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Intents
{

    [Authorize]

    public class ModifyIntentNameController : PalavyrBaseController
    {

        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<ModifyIntentNameController> logger;

        public ModifyIntentNameController(
            IConfigurationRepository configurationRepository,
            ILogger<ModifyIntentNameController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpPut("areas/update/name/{areaId}")]
        public async Task<string> Modify(
            [FromBody] UpdateAreaNameRequest areaNameText,
            string areaId
        )
        {
            var area = await configurationRepository.GetAreaById(areaId);
            if (areaNameText.AreaName != area.AreaName)
            {
                area.AreaName = areaNameText.AreaName;
                await configurationRepository.CommitChangesAsync();
            }
            return areaNameText.AreaName;
        }
    }
}