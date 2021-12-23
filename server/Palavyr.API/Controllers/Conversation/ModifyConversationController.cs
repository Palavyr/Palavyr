using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation;
using Palavyr.Core.Models.Resources.Requests;

namespace Palavyr.API.Controllers.Conversation
{
    public class ModifyConversationController : PalavyrBaseController
    {
        private ILogger<ModifyConversationController> logger;
        private readonly IConversationNodeUpdater conversationNodeUpdater;

        public ModifyConversationController(
            ILogger<ModifyConversationController> logger,
            IConversationNodeUpdater conversationNodeUpdater
        )
        {
            this.logger = logger;
            this.conversationNodeUpdater = conversationNodeUpdater;
        }

        [HttpPut("configure-conversations/{areaId}")]
        public async Task<List<ConversationNode>> Modify(
            [FromRoute] string areaId,
            [FromBody] ConversationNodeDto update,
            CancellationToken cancellationToken
            )
        {
            var updatedConvo = await conversationNodeUpdater.UpdateConversation(areaId, update.Transactions, cancellationToken);
            return updatedConvo;
        }
    }
}