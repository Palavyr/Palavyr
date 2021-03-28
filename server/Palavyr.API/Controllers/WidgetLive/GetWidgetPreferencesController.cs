using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.AuthenticationServices;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class GetWidgetPreferencesController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetWidgetPreferencesController> logger;

        public GetWidgetPreferencesController(IConfigurationRepository configurationRepository,  ILogger<GetWidgetPreferencesController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpGet("widget/preferences")]
        public async Task<WidgetPreference> FetchPreferences([FromHeader] string accountId)
        {
            return await configurationRepository.GetWidgetPreferences(accountId);
        }
    }
}