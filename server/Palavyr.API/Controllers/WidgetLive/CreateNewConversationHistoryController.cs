using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.AuthenticationServices;
using Palavyr.Data.Abstractions;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    [Route("api")]
    [ApiController]
    public class CreateNewConversationHistoryController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private ILogger<CreateNewConversationHistoryController> logger;

        public CreateNewConversationHistoryController(
            IDashConnector dashConnector,
            ILogger<CreateNewConversationHistoryController> logger)
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpGet("widget/{areaId}/create")]
        public async Task<NewConversation> Create([FromHeader] string accountId, [FromRoute] string areaId)
        {
            logger.LogDebug("Fetching Preferences...");
            var widgetPreference = await dashConnector.GetWidgetPreferences(accountId);

            logger.LogDebug("Fetching nodes...");
            var incompleteNodeList = await dashConnector.GetAreaConversationNodes(accountId, areaId);

            var convoNodes = EndingSequence.AttachEndingSequenceToNodeList(incompleteNodeList, areaId, accountId);

            logger.LogDebug("Creating new conversation for user with apikey: {apiKey}");
            var newConvo = NewConversation.CreateNew(widgetPreference, convoNodes);

            return newConvo;
        }
    }
}