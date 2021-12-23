using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class UpdateChatHistoryController : PalavyrBaseController
    {
        private ConvoContext convoContext;
        private readonly IHoldAnAccountId accountIdHolder;

        public UpdateChatHistoryController(ConvoContext convoContext, ILogger<UpdateChatHistoryController> logger, IHoldAnAccountId accountIdHolder)
        {
            this.convoContext = convoContext;
            this.accountIdHolder = accountIdHolder;
        }

        [HttpPost("widget/conversation")]
        public async Task<IActionResult> Modify(ConversationHistory history)
        {
            var conversationUpdate = history.CreateFromPartial(accountIdHolder.AccountId);
            convoContext.ConversationHistories.Add(conversationUpdate);
            await convoContext.SaveChangesAsync();
            return NoContent();
        }
    }
}