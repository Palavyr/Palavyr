using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.AuthenticationServices;
using Server.Domain.Conversation;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
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