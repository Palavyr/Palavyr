using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain;
using Palavyr.Services.AuthenticationServices;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class CreateNewConversationHistoryController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<CreateNewConversationHistoryController> logger;

        public CreateNewConversationHistoryController(
            IConfigurationRepository configurationRepository,
            ILogger<CreateNewConversationHistoryController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpGet("widget/{areaId}/create")]
        public async Task<NewConversation> Create([FromHeader] string accountId, [FromRoute] string areaId)
        {
            logger.LogDebug("Fetching Preferences...");
            var widgetPreference = await configurationRepository.GetWidgetPreferences(accountId);

            logger.LogDebug("Fetching nodes...");
            var incompleteNodeList = await configurationRepository.GetAreaConversationNodes(accountId, areaId);

            var convoNodes = EndingSequence.AttachEndingSequenceToNodeList(incompleteNodeList, areaId, accountId);

            logger.LogDebug("Creating new conversation for user with apikey: {apiKey}");
            var newConvo = NewConversation.CreateNew(widgetPreference, convoNodes);

            return newConvo;
        }
    }
}