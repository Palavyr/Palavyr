using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.conversations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.WidgetScheme)]
    [Route("api")]
    [ApiController]
    public class UpdateChatHistoryController : ControllerBase
    {
        private ConvoContext convoContext;

        public UpdateChatHistoryController(ConvoContext convoContext, ILogger<UpdateChatHistoryController> logger)
        {
            this.convoContext = convoContext;
        }

        [HttpPost("widget/conversation")]
        public async Task<IActionResult> Modify([FromHeader] string accountId, ConversationUpdate update)
        {
            var conversationUpdate = update.CreateFromPartial(accountId);
            convoContext.Conversations.Add(conversationUpdate);
            await convoContext.SaveChangesAsync();
            return NoContent();
        }
    }
}