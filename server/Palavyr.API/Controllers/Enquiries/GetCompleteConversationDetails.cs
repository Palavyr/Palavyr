using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Domain.Conversation.Schemas;


namespace Palavyr.API.Controllers.Enquiries
{

    public class GetCompleteConversationDetails : PalavyrBaseController
    {
        private readonly ILogger<GetCompleteConversationDetails> logger;
        private readonly ConvoContext convoContext;

        public GetCompleteConversationDetails(
            ILogger<GetCompleteConversationDetails> logger,
            ConvoContext convoContext 
        )
        {
            this.logger = logger;
            this.convoContext = convoContext;
        }

        [ResponseCache(Duration = 3600)]
        [HttpGet("enquiries/review/{conversationId}")]
        public async Task<ConversationUpdate[]> Get([FromHeader] string accountId, [FromRoute] string conversationId)
        {
            logger.LogDebug("Collecting Conversation for viewing...");
            var convoRows = await convoContext
                .Conversations
                .Where(row => row.ConversationId == conversationId)
                .ToListAsync();

            convoRows.Sort((x, y) => x.Id > y.Id ? 1 : -1);
            return convoRows.ToArray();
        }
    }
}