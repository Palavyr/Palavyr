using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Data.Abstractions;
using Palavyr.Domain.Configuration.Schemas;


namespace Palavyr.API.controllers.Conversation
{
    [Route("api")]
    [ApiController]
    public class ModifyConversationNodeController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private ILogger<ModifyConversationNodeController> logger;

        public ModifyConversationNodeController(
            IDashConnector dashConnector,
            ILogger<ModifyConversationNodeController> logger
        )
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpPut("configure-conversations/{areaId}/nodes/{nodeId}")]
        public async Task<List<ConversationNode>> Modify(
            [FromHeader] string accountId,
            [FromRoute] string nodeId,
            [FromRoute] string areaId,
            [FromBody] ConversationNode newNode)
        {
            var updatedConversation = await dashConnector.UpdateConversationNode(accountId, areaId, nodeId, newNode);
            await dashConnector.CommitChangesAsync();
            return updatedConversation;
        }
    }
}