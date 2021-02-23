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
    [Route("api")]
    [ApiController]
    public class GetWidgetPreferencesController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private ILogger<GetWidgetPreferencesController> logger;

        public GetWidgetPreferencesController(IDashConnector dashConnector,  ILogger<GetWidgetPreferencesController> logger)
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpGet("widget/preferences")]
        public async Task<WidgetPreference> FetchPreferences([FromHeader] string accountId)
        {
            return await dashConnector.GetWidgetPreferences(accountId);
        }
    }
}