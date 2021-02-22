using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.AuthenticationServices;
using Palavyr.Data.Abstractions;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    [Route("api")]
    [ApiController]
    public class GetConfiguredAreasController : ControllerBase
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