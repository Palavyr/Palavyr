using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

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
        public async Task<WidgetPreference> GetWidgetPreferences()
        {
            return await configurationRepository.GetWidgetPreferences();
        }
    }
}