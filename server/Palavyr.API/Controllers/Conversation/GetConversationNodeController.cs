using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.controllers.Conversation
{

    public class GetConversationNodeController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetConversationNodeController> logger;

        public GetConversationNodeController(
            IConfigurationRepository configurationRepository,
            ILogger<GetConversationNodeController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }
        
        [HttpGet("configure-conversations/nodes/{nodeId}")]
        public async Task<ConversationNode> Get([FromRoute] string nodeId)
        {
            // node Ids are globally unique - don't need account Id Filter
            var node = await configurationRepository.GetConversationNodeById(nodeId);
            return node;
        }
    }
}