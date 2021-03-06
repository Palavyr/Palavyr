using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
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

        public ModifyConversationController(
            ILogger<ModifyConversationController> logger,
            IDashConnector dashConnector
        )
        {
            this.logger = logger;
            this.dashConnector = dashConnector;
        }

        [HttpPut("configure-conversations/{areaId}")]
        public async Task<List<ConversationNode>> Modify(
            [FromHeader] string accountId, 
            [FromRoute] string areaId, 
            [FromBody] ConversationNodeDto update)
        {

            dashConnector.RemoveAreaNodes(areaId, accountId);
            // dashConnector.RemoveNodeRangeByIds(update.IdsToDelete);
            var area = await dashConnector.GetAreaWithConversationNodes(accountId, areaId);
            var mappedUpdates = ConversationNode.MapUpdate(accountId, update.Transactions);
            
            area.ConversationNodes.AddRange(mappedUpdates);
            await dashConnector.CommitChangesAsync();

            var newNodes = await dashConnector.GetAreaConversationNodes(accountId, areaId); 
            return newNodes;
        }
    }
}