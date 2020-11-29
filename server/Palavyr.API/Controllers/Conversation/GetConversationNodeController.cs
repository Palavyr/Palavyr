using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.controllers.Conversation
{
    [Route("api")]
    [ApiController]
    public class GetConversationNodeController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<GetConversationNodeController> logger;

        public GetConversationNodeController(
            DashContext dashContext,
            ILogger<GetConversationNodeController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
        
        [HttpGet("configure-conversations/nodes/{nodeId}")]
        public ConversationNode Get([FromRoute] string nodeId)
        {
            // node Ids are globally unique - don't need account Id Filter
            var result = dashContext.ConversationNodes.Single(row => row.NodeId == nodeId);
            logger.LogDebug($"Retrieving Conversation Node {result.NodeId}");
            return result;
        }
    }
}