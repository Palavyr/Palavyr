using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class UpdateChatHistoryController : PalavyrBaseController
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