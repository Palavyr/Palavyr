using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers.Conversation;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Conversation;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.controllers.Conversation
{
    [Route("api")]
    [ApiController]
    public class ModifyConversationController : ControllerBase
    {
        private ILogger<ModifyConversationController> logger;
        private readonly IDashConnector dashConnector;
        private readonly OrphanRemover orphanRemover;

        public ModifyConversationController(
            ILogger<ModifyConversationController> logger,
            IDashConnector dashConnector,
            OrphanRemover orphanRemover
        )
        {
            this.logger = logger;
            this.dashConnector = dashConnector;
            this.orphanRemover = orphanRemover;
        }

        [HttpPut("configure-conversations/{areaId}")]
        public async Task<List<ConversationNode>> Modify(
            [FromHeader] string accountId, 
            [FromRoute] string areaId, 
            [FromBody] ConversationNodeDto update)
        {
            // TODO: This makes 3 calls. Confirm that we only need to make 1 call.
            dashConnector.RemoveAreaNodes(areaId, accountId);
            var area = await dashConnector.GetAreaWithConversationNodes(accountId, areaId);
            var mappedUpdates = ConversationNode.MapUpdate(accountId, update.Transactions);

            area.ConversationNodes.AddRange(mappedUpdates);

            await dashConnector.CommitChangesAsync();

            var updatedArea = await dashConnector.GetAreaWithConversationNodes(accountId, areaId);
            var deOrphanedAreaConvo = orphanRemover.RemoveOrphanedNodes(updatedArea.ConversationNodes);
            updatedArea.ConversationNodes = deOrphanedAreaConvo;
            await dashConnector.CommitChangesAsync();

            return await dashConnector.GetAreaConversationNodes(accountId, areaId);
        }
    }
}