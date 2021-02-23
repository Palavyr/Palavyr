using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
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
        private DashContext dashContext;
        private readonly IDashConnector dashConnector;

        public ModifyConversationController(
            ILogger<ModifyConversationController> logger,
            DashContext dashContext,
            IDashConnector dashConnector
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
            this.dashConnector = dashConnector;
        }

        [HttpPut("configure-conversations/{areaId}")]
        public async Task<List<ConversationNode>> Modify(
            [FromHeader] string accountId, 
            [FromRoute] string areaId, 
            [FromBody] ConversationConfigurationUpdate update)
        {

            dashConnector.RemoveNodeRangeByIds(update.IdsToDelete);
            var area = await dashConnector.GetAreaWithConversationNodes(accountId, areaId);
            var mappedUpdates = ConversationNode.MapUpdate(accountId, update.Transactions);
            
            area.ConversationNodes.AddRange(mappedUpdates);
            await dashConnector.CommitChangesAsync();

            var newNodes = await dashConnector.GetAreaConversationNodes(accountId, areaId); 
            return newNodes;
        }
    }
}