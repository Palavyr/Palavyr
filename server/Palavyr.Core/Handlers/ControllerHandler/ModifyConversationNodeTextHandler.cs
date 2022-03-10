using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyConversationNodeTextHandler : IRequestHandler<ModifyConversationNodeTextRequest, ModifyConversationNodeTextResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;

        public ModifyConversationNodeTextHandler(IEntityStore<ConversationNode> convoNodeStore)
        {
            this.convoNodeStore = convoNodeStore;
        }

        public async Task<ModifyConversationNodeTextResponse> Handle(ModifyConversationNodeTextRequest request, CancellationToken cancellationToken)
        {
            var updatedConversationNode = await convoNodeStore.UpdateConversationNodeText(request.IntentId, request.NodeId, request.UpdatedNodeText);
            return new ModifyConversationNodeTextResponse(updatedConversationNode);
        }
    }

    public class ModifyConversationNodeTextResponse
    {
        public ModifyConversationNodeTextResponse(ConversationNode response) => Response = response;
        public ConversationNode Response { get; set; }
    }

    public class ModifyConversationNodeTextRequest : IRequest<ModifyConversationNodeTextResponse>
    {
        public string UpdatedNodeText { get; set; }
        public string NodeId { get; set; }
        public string IntentId { get; set; }
    }
}