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