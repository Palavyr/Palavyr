using System.Threading.Tasks;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.controllers.Conversation
{
    [Route("api")]
    [ApiController]
    public class GetConversationNodeController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private ILogger<GetConversationNodeController> logger;

        public GetConversationNodeController(
            IDashConnector dashConnector,
            ILogger<GetConversationNodeController> logger
        )
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }
        
        [HttpGet("configure-conversations/nodes/{nodeId}")]
        public async Task<ConversationNode> Get([FromRoute] string nodeId)
        {
            // node Ids are globally unique - don't need account Id Filter
            var node = await dashConnector.GetConversationNodeById(nodeId);
            return node;
        }
    }
}