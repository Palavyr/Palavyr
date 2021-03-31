using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.controllers.Conversation
{

    public class GetConversationByAreaIdController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetConversationByAreaIdController> logger;

        public GetConversationByAreaIdController(
            
            IConfigurationRepository configurationRepository,
            ILogger<GetConversationByAreaIdController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }
        
        [HttpGet("configure-conversations/{areaId}")]
        public async Task<List<ConversationNode>> Get(
            [FromHeader] string accountId, 
            [FromRoute] string areaId)
        {
            var conversation = await configurationRepository.GetAreaConversationNodes(accountId, areaId);
            return conversation;
        }
    }
}