using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Intents
{
    [Authorize]
    public class ModifyIntentNameController : PalavyrBaseController
    {
        private readonly IEntityStore<Area> intentStore;
        private readonly ILogger<ModifyIntentNameController> logger;

        public ModifyIntentNameController(
            IEntityStore<Area> intentStore,
            ILogger<ModifyIntentNameController> logger
        )
        {
            this.intentStore = intentStore;
            this.logger = logger;
        }

        [HttpPut("areas/update/name/{intentId}")]
        public async Task<string> Modify(
            [FromBody]
            UpdateIntentNameRequest intentNameText,
            string intentId
        )
        {
            var intent = await intentStore.Get(intentId, s => s.AreaIdentifier);
            if (intentNameText.AreaName != intent.AreaName)
            {
                intent.AreaName = intentNameText.AreaName;
            }

            return intentNameText.AreaName;
        }
    }
}