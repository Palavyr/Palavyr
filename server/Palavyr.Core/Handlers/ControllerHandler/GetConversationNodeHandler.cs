using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetConversationNodeHandler : IRequestHandler<GetConversationNodeRequest, GetConversationNodeResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IMapToNew<ConversationNode, ConversationNodeResource> mapper;

        public GetConversationNodeHandler(IEntityStore<ConversationNode> convoNodeStore, IMapToNew<ConversationNode, ConversationNodeResource> mapper)
        {
            this.convoNodeStore = convoNodeStore;
            this.mapper = mapper;
        }

        public async Task<GetConversationNodeResponse> Handle(GetConversationNodeRequest request, CancellationToken cancellationToken)
        {
            // node Ids are globally unique - don't need account Id Filter
            var node = await convoNodeStore.Get(request.NodeId, s => s.NodeId);

            var resource = await mapper.Map(node);
            return new GetConversationNodeResponse(resource);
        }
    }

    public class GetConversationNodeResponse
    {
        public GetConversationNodeResponse(ConversationNodeResource response) => Response = response;
        public ConversationNodeResource Response { get; set; }
    }

    public class GetConversationNodeRequest : IRequest<GetConversationNodeResponse>
    {
        public GetConversationNodeRequest(string nodeId)
        {
            NodeId = nodeId;
        }

        public string NodeId { get; set; }
    }
}