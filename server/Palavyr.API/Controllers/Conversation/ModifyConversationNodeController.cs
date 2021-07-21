using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Conversation
{
    public class ModifyConversationNodeController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<ModifyConversationNodeController> logger;

        public ModifyConversationNodeController(
            IConfigurationRepository configurationRepository,
            ILogger<ModifyConversationNodeController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpPut("configure-conversations/{areaId}/nodes/{nodeId}")]
        public async Task<List<ConversationNode>> Modify(
            [FromHeader]
            string accountId,
            [FromRoute]
            string nodeId,
            [FromRoute]
            string areaId,
            [FromBody]
            ConversationNode newNode)
        {
            var updatedConversation = await configurationRepository.UpdateConversationNode(accountId, areaId, nodeId, newNode);
            await configurationRepository.CommitChangesAsync();
            return updatedConversation;
        }
    }
}