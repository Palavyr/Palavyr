using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetConversationHandler : IRequestHandler<GetConversationRequest, GetConversationResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;

        public GetConversationHandler(IEntityStore<ConversationNode> convoNodeStore)
        {
            this.convoNodeStore = convoNodeStore;
        }

        public async Task<GetConversationResponse> Handle(GetConversationRequest request, CancellationToken cancellationToken)
        {
            var conversation = await convoNodeStore.GetMany(request.IntentId, s => s.AreaIdentifier);
            return new GetConversationResponse(conversation);
        }
    }

    public class GetConversationResponse
    {
        public GetConversationResponse(List<ConversationNode> response) => Response = response;
        public List<ConversationNode> Response { get; set; }
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