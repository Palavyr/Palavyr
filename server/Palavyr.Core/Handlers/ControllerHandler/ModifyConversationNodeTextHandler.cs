using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyConversationNodeTextHandler : IRequestHandler<ModifyConversationNodeTextRequest, ModifyConversationNodeTextResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IMapToNew<ConversationNode, ConversationDesignerNodeResource> mapper;

        public ModifyConversationNodeTextHandler(IEntityStore<ConversationNode> convoNodeStore, IMapToNew<ConversationNode, ConversationDesignerNodeResource> mapper)
        {
            this.convoNodeStore = convoNodeStore;
            this.mapper = mapper;
        }

        public async Task<ModifyConversationNodeTextResponse> Handle(ModifyConversationNodeTextRequest request, CancellationToken cancellationToken)
        {
            var updatedConversationNode = await convoNodeStore.UpdateConversationNodeText(request.IntentId, request.NodeId, request.UpdatedNodeText);
            var resource = await mapper.Map(updatedConversationNode, cancellationToken);
            return new ModifyConversationNodeTextResponse(resource);
        }
    }

    public class ModifyConversationNodeTextResponse
    {
        public ModifyConversationNodeTextResponse(ConversationDesignerNodeResource response) => Response = response;
        public ConversationDesignerNodeResource Response { get; set; }
    }

    public class ModifyConversationNodeTextRequest : IRequest<ModifyConversationNodeTextResponse>
    {
        public string UpdatedNodeText { get; set; }
        public string NodeId { get; set; }
        public string IntentId { get; set; }
    }
}