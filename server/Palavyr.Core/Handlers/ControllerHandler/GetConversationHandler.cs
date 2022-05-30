using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetConversationHandler : IRequestHandler<GetConversationRequest, GetConversationResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IMapToNew<ConversationNode, ConversationDesignerNodeResource> mapper;

        public GetConversationHandler(IEntityStore<ConversationNode> convoNodeStore, IMapToNew<ConversationNode, ConversationDesignerNodeResource> mapper)
        {
            this.convoNodeStore = convoNodeStore;
            this.mapper = mapper;
        }

        public async Task<GetConversationResponse> Handle(GetConversationRequest request, CancellationToken cancellationToken)
        {
            var conversation = await convoNodeStore.GetMany(request.IntentId, s => s.AreaIdentifier);

            var resource = await mapper.MapMany(conversation, cancellationToken);
            return new GetConversationResponse(resource);
        }
    }

    public class GetConversationResponse
    {
        public GetConversationResponse(IEnumerable<ConversationDesignerNodeResource> response) => Response = response;
        public IEnumerable<ConversationDesignerNodeResource> Response { get; set; }
    }

    public class GetConversationRequest : IRequest<GetConversationResponse>
    {
        public GetConversationRequest(string intendId)
        {
            IntentId = intendId;
        }

        public string IntentId { get; set; }
    }
}