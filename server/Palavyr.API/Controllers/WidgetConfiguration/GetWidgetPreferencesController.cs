using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.WidgetConfiguration
{

    public class GetWidgetPreferencesController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetWidgetPreferencesController> logger;

        public GetWidgetPreferencesController(IConfigurationRepository configurationRepository, ILogger<GetWidgetPreferencesController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpGet("widget-config/preferences")]
        public async Task<WidgetPreference> GetWidgetPreferences([FromHeader] string accountId)
        {
            return await configurationRepository.GetWidgetPreferences(accountId);
        }
    }
}