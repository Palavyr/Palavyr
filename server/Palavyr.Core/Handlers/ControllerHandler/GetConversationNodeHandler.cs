using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetConversationNodeHandler : IRequestHandler<GetConversationNodeRequest, GetConversationNodeResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;

        public GetConversationNodeHandler(IEntityStore<ConversationNode> convoNodeStore)
        {
            this.convoNodeStore = convoNodeStore;
        }

        public async Task<GetConversationNodeResponse> Handle(GetConversationNodeRequest request, CancellationToken cancellationToken)
        {
            // node Ids are globally unique - don't need account Id Filter
            var node = await convoNodeStore.Get(request.NodeId, s => s.NodeId);
            return new GetConversationNodeResponse(node);
        }
    }

    public class GetConversationNodeResponse
    {
        public GetConversationNodeResponse(ConversationNode response) => Response = response;
        public ConversationNode Response { get; set; }
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