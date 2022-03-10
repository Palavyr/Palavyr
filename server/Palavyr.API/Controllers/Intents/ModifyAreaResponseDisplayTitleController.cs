using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Intents
{
    [Authorize]
    public class ModifyAreaResponseDisplayTitleController : PalavyrBaseController
    {
        private readonly IEntityStore<Area> intentStore;
        private ILogger<ModifyAreaResponseDisplayTitleController> logger;

        public ModifyAreaResponseDisplayTitleController(
            IEntityStore<Area> intentStore,
            ILogger<ModifyAreaResponseDisplayTitleController> logger
        )
        {
            this.intentStore = intentStore;
            this.logger = logger;
        }

        [HttpPut("areas/update/display-title/{areaId}")]
        public async Task<string> Modify(
            [FromBody]
            AreaDisplayTitleText areaDisplayTitleText,
            string areaId
        )
        {
            var area = await intentStore.Get(areaId, s => s.AreaIdentifier);
            if (areaDisplayTitleText.AreaDisplayTitle != null)
            {
                area.AreaDisplayTitle = areaDisplayTitleText.AreaDisplayTitle;
            }

            return areaDisplayTitleText.AreaDisplayTitle;
        }
    }
}