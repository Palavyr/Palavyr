using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.controllers.Conversation
{

    public class GetConversationByAreaIdController : PalavyrBaseController
    {
        private readonly IDashConnector dashConnector;
        private ILogger<GetConversationByAreaIdController> logger;

        public GetConversationByAreaIdController(
            
            IDashConnector dashConnector,
            ILogger<GetConversationByAreaIdController> logger
        )
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }
        
        [HttpGet("configure-conversations/{areaId}")]
        public async Task<List<ConversationNode>> Get(
            [FromHeader] string accountId, 
            [FromRoute] string areaId)
        {
            var conversation = await dashConnector.GetAreaConversationNodes(accountId, areaId);
            return conversation;
        }
    }
}