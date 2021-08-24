using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class CreateNewConversationHistoryController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConvoHistoryRepository convoRepository;
        private ILogger<CreateNewConversationHistoryController> logger;

        public CreateNewConversationHistoryController(
            IConfigurationRepository configurationRepository,
            IConvoHistoryRepository convoRepository,
            ILogger<CreateNewConversationHistoryController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.convoRepository = convoRepository;
            this.logger = logger;
        }

        [HttpPost("widget/{areaId}/create")]
        public async Task<NewConversation> Create(
            [FromHeader]
            string accountId,
            [FromRoute]
            string areaId,
            [FromBody]
            ConversationRecordUpdate recordUpdate
        )
        {
            logger.LogDebug("Fetching nodes...");
            var standardNodes = await configurationRepository.GetAreaConversationNodes(accountId, areaId);
            var completeConversation = EndingSequence.AttachEndingSequenceToNodeList(standardNodes, areaId, accountId);

            logger.LogDebug("Creating new conversation for user with apikey: {apiKey}");
            var widgetNodes = completeConversation.MapConversationToWidgetNodes();

            var newConvo = NewConversation.CreateNew(widgetNodes);

            var area = await configurationRepository.GetAreaById(accountId, areaId);
            var newConversationRecord = ConversationRecord.CreateDefault(newConvo.ConversationId, accountId, area.AreaName, areaId);

            if (!string.IsNullOrEmpty(recordUpdate.Email))
            {
                newConversationRecord.Email = recordUpdate.Email;
            }

            if (!string.IsNullOrEmpty(recordUpdate.Name))
            {
                newConversationRecord.Name = recordUpdate.Name;
            }

            await convoRepository.CreateNewConversationRecord(newConversationRecord);

            await convoRepository.CommitChangesAsync();
            await configurationRepository.CommitChangesAsync();

            return newConvo;
        }
    }
}