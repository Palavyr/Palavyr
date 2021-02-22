using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Data.Abstractions;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.WidgetConfiguration
{
    [Route("api")]
    [ApiController]
    public class GetWidgetPreferencesController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private ILogger<GetWidgetPreferencesController> logger;

        public GetWidgetPreferencesController(IDashConnector dashConnector, ILogger<GetWidgetPreferencesController> logger)
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpGet("widget-config/preferences")]
        public async Task<WidgetPreference> GetWidgetPreferences([FromHeader] string accountId)
        {
            return await dashConnector.GetWidgetPreferences(accountId);
        }
    }
}