using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.ConversationServices;

namespace Palavyr.API.Controllers.Enquiries
{

    public class ModifyCompletedConversationsController : PalavyrBaseController
    {
        private readonly CompletedConversationModifier completedConversationModifier;
        private ILogger<ModifyCompletedConversationsController> logger;

        public ModifyCompletedConversationsController(
            CompletedConversationModifier completedConversationModifier,
            ILogger<ModifyCompletedConversationsController> logger
        )
        {
            this.completedConversationModifier = completedConversationModifier;
            this.logger = logger;
        }

        [HttpPut("enquiries/update/{conversationId}")]
        public async Task<Enquiry[]> UpdateCompletedConversation([FromHeader] string accountId, string conversationId)
        {
            var modifiedCompletedConversation = await completedConversationModifier.ModifyCompletedConversation(accountId, conversationId);
            return modifiedCompletedConversation;
        }
    }
}