using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ConversationServices;

namespace Palavyr.API.Controllers.Enquiries
{

    public class ModifyCompletedConversationsController : PalavyrBaseController
    {
        private readonly ICompletedConversationModifier completedConversationModifier;
        private ILogger<ModifyCompletedConversationsController> logger;

        public ModifyCompletedConversationsController(
            ICompletedConversationModifier completedConversationModifier,
            ILogger<ModifyCompletedConversationsController> logger
        )
        {
            this.completedConversationModifier = completedConversationModifier;
            this.logger = logger;
        }

        [HttpPut("enquiries/update/{conversationId}")]
        public async Task<Enquiry[]> UpdateCompletedConversation(string conversationId,CancellationToken cancellationToken)
        {
            var modifiedCompletedConversation = await completedConversationModifier.ModifyCompletedConversation(conversationId);
            return modifiedCompletedConversation;
        }
    }
}