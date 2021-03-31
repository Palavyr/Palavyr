using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.controllers.Conversation
{

    public class ModifyConversationController : PalavyrBaseController
    {
        private ILogger<ModifyConversationController> logger;
        private readonly IConfigurationRepository configurationRepository;
        private readonly OrphanRemover orphanRemover;

        public ModifyConversationController(
            ILogger<ModifyConversationController> logger,
            IConfigurationRepository configurationRepository,
            OrphanRemover orphanRemover
        )
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
            this.orphanRemover = orphanRemover;
        }

        [HttpPut("configure-conversations/{areaId}")]
        public async Task<List<ConversationNode>> Modify(
            [FromHeader] string accountId, 
            [FromRoute] string areaId, 
            [FromBody] ConversationNodeDto update)
        {
            // TODO: This makes 3 calls. Confirm that we only need to make 1 call.
            configurationRepository.RemoveAreaNodes(areaId, accountId);
            var area = await configurationRepository.GetAreaWithConversationNodes(accountId, areaId);
            var mappedUpdates = ConversationNode.MapUpdate(accountId, update.Transactions);

            area.ConversationNodes.AddRange(mappedUpdates);

            await configurationRepository.CommitChangesAsync();

            var updatedArea = await configurationRepository.GetAreaWithConversationNodes(accountId, areaId);
            var deOrphanedAreaConvo = orphanRemover.RemoveOrphanedNodes(updatedArea.ConversationNodes);
            updatedArea.ConversationNodes = deOrphanedAreaConvo;
            await configurationRepository.CommitChangesAsync();

            return await configurationRepository.GetAreaConversationNodes(accountId, areaId);
        }
    }
}