using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Conversation
{
    public class ModifyConversationNodeTextController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<ModifyConversationNodeTextController> logger;

        public ModifyConversationNodeTextController(
            IConfigurationRepository configurationRepository,
            ILogger<ModifyConversationNodeTextController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpPut("configure-conversations/{areaId}/nodes/{nodeId}/text")]
        public async Task<ConversationNode> Modify(

            [FromRoute]
            string nodeId,
            [FromRoute]
            string areaId,
            [FromBody]
            UpdatedNodeTextRequest nodeTextUpdate)
        {
            var updatedConversationNode = await configurationRepository.UpdateConversationNodeText(areaId, nodeId, nodeTextUpdate.UpdatedNodeText);
            return updatedConversationNode;
        }
    }

    public class UpdatedNodeTextRequest
    {
        public string UpdatedNodeText { get; set; }
    }
}