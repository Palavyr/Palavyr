using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class GetConfiguredAreasController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetConfiguredAreasController> logger;

        public GetConfiguredAreasController(
            IConfigurationRepository configurationRepository,
            ILogger<GetConfiguredAreasController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpGet("widget/areas")]
        public async Task<List<Area>> Get([FromHeader] string accountId)
        {
            logger.LogDebug("Collecting configured areas for live-widget");

            var activeAreas = await configurationRepository.GetActiveAreas(accountId);
            return activeAreas;
        }
    }
}