using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.WidgetScheme)]
    [Route("api")]
    [ApiController]
    public class CreateNewConversationHistoryController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<CreateNewConversationHistoryController> logger;

        public CreateNewConversationHistoryController(DashContext dashContext, ILogger<CreateNewConversationHistoryController> logger)
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
        
        [HttpGet("widget/{areaId}/create")]
        public async Task<IActionResult> Create([FromHeader] string accountId, [FromRoute] string areaId)
        {
            logger.LogDebug("Fetching Preferences...");
            var widgetPreference = await dashContext.WidgetPreferences.SingleOrDefaultAsync(row => row.AccountId == accountId);

            logger.LogDebug("Fetching nodes...");
            var incompleteNodeList = dashContext
                .ConversationNodes
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToList();
            var convoNodes = EndingSequence.AttachEndingSequenceToNodeList(incompleteNodeList, areaId, accountId);

            logger.LogDebug("Creating new conversation for user with apikey: {apiKey}");
            var newConvo = NewConversation.CreateNew(widgetPreference, convoNodes);

            return Ok(newConvo);
        }
    }
}