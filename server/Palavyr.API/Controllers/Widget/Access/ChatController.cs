using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.conversations;
using Microsoft.AspNetCore.Authorization;

namespace Palavyr.API.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.WidgetScheme)]
    [Route("api/widget")]
    [ApiController]
    public class ChatController : BaseController
    {
        public ChatController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }
        
        [HttpPost("conversation")]
        public StatusCodeResult UpdateActiveConversation([FromHeader] string accountId, ConversationUpdate update)
        {
            var conversationUpdate = update.CreateFromPartial(accountId);
            ConvoContext.Conversations.Add(conversationUpdate);
            ConvoContext.SaveChanges();
            return new OkResult();
        }
    }
}