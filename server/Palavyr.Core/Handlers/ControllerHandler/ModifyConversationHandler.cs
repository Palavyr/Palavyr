using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Conversation;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyConversationHandler : IRequestHandler<ModifyConversationRequest, ModifyConversationResponse>
    {
        private readonly IConversationNodeUpdater conversationNodeUpdater;
        private readonly IMapToNew<ConversationNode, ConversationDesignerNodeResource> resourceMapper;
        private readonly IMapToNew<ConversationDesignerNodeResource, ConversationNode> modelMapper;

        public ModifyConversationHandler(
            IConversationNodeUpdater conversationNodeUpdater,
            IMapToNew<ConversationDesignerNodeResource, ConversationNode> modelMapper,
            IMapToNew<ConversationNode, ConversationDesignerNodeResource> resourceMapper // TODO: This should be a IMapToExisting
        )
        {
            this.conversationNodeUpdater = conversationNodeUpdater;
            this.resourceMapper = resourceMapper;
            this.modelMapper = modelMapper;
        }

        public async Task<ModifyConversationResponse> Handle(ModifyConversationRequest request, CancellationToken cancellationToken)
        {
            var mappedUpdates = await modelMapper.MapMany(request.Transactions, cancellationToken);

            var updatedConvo = await conversationNodeUpdater.UpdateDesignerConversationForIntent(request.IntentId, mappedUpdates, cancellationToken);

            var resource = await resourceMapper.MapMany(updatedConvo, cancellationToken);
            return new ModifyConversationResponse(resource);
        }
    }

    public class ModifyConversationResponse
    {
        public ModifyConversationResponse(IEnumerable<ConversationDesignerNodeResource> response) => Response = response;
        public IEnumerable<ConversationDesignerNodeResource> Response { get; set; }
    }

    public class ModifyConversationRequest : IRequest<ModifyConversationResponse>
    {
        public IEnumerable<ConversationDesignerNodeResource> Transactions { get; set; }
        public string IntentId { get; set; }
    }
}