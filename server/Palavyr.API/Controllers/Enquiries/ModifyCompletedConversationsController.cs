using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ModifyCompletedConversationsController : ControllerBase
    {
        private ConvoContext convoContext;
        private ILogger<ModifyCompletedConversationsController> logger;

        public ModifyCompletedConversationsController(
            ConvoContext convoContext,
            ILogger<ModifyCompletedConversationsController> logger
        )
        {
            this.convoContext = convoContext;
            this.logger = logger;
        }

        [HttpPut("enquiries/update/{conversationId}")]
        public async Task<IActionResult> UpdateCompletedConversation(string conversationId)
        {
            var convo = await convoContext.CompletedConversations.SingleOrDefaultAsync(row =>
                row.ConversationId == conversationId);
            convo.Seen = !convo.Seen;
            await convoContext.SaveChangesAsync();
            return NoContent();
        }
    }
}