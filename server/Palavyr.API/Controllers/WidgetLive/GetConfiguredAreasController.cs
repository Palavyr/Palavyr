using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.AuthenticationServices;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class GetConfiguredAreasController : PalavyrBaseController
    {
        private readonly IDashConnector dashConnector;
        private ILogger<GetConfiguredAreasController> logger;

        public GetConfiguredAreasController(
            IDashConnector dashConnector,
            ILogger<GetConfiguredAreasController> logger
        )
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpGet("widget/areas")]
        public async Task<List<Area>> Get([FromHeader] string accountId)
        {
            logger.LogDebug("Collecting configured areas for live-widget");

            var activeAreas = await dashConnector.GetActiveAreas(accountId);
            return activeAreas;
        }
    }
}