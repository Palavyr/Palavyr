using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyConversationHandler : IRequestHandler<ModifyConversationRequest, ModifyConversationResponse>
    {
        private readonly IConversationNodeUpdater conversationNodeUpdater;

        public ModifyConversationHandler(
            IConversationNodeUpdater conversationNodeUpdater
        )
        {
            this.conversationNodeUpdater = conversationNodeUpdater;
        }

        public async Task<ModifyConversationResponse> Handle(ModifyConversationRequest request, CancellationToken cancellationToken)
        {
            var updatedConvo = await conversationNodeUpdater.UpdateConversation(request.IntentId, request.Transactions, cancellationToken);
            return new ModifyConversationResponse(updatedConvo);
        }
    }

    public class ModifyConversationResponse
    {
        public ModifyConversationResponse(List<ConversationNode> response) => Response = response;
        public List<ConversationNode> Response { get; set; }
    }

    public class ModifyConversationRequest : IRequest<ModifyConversationResponse>
    {
        public List<ConversationNode> Transactions { get; set; }
        public string IntentId { get; set; }
    }
}