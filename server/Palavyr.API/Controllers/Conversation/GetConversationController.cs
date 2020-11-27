using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.controllers.Conversation
{
    [Route("api")]
    [ApiController]
    public class GetConversationByAreaIdController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<GetConversationByAreaIdController> logger;

        public GetConversationByAreaIdController(
            DashContext dashContext,
            ILogger<GetConversationByAreaIdController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
        
        [HttpGet("configure-conversations/{areaId}")]
        public IActionResult Get(
            [FromHeader] string accountId, 
            [FromRoute] string areaId)
        {
            var conversation = dashContext
                .ConversationNodes
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToList();
            return Ok(conversation);
        }
        
    }
}