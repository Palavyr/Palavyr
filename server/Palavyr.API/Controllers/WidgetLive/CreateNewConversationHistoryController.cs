using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;

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
            logger.LogDebug("Fetching nodes...");
            var standardNodes = await configurationRepository.GetAreaConversationNodes(accountId, areaId);
            var completeConversation = EndingSequence.AttachEndingSequenceToNodeList(standardNodes, areaId, accountId);

            logger.LogDebug("Creating new conversation for user with apikey: {apiKey}");
            var widgetNodes = completeConversation.MapConversationToWidgetNodes();
            var newConvo = NewConversation.CreateNew(widgetNodes);
            
            return newConvo;
        }
    }
}