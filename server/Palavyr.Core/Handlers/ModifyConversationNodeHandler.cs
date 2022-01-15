using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class ModifyConversationNodeHandler : IRequestHandler<ModifyConversationNodeRequest, ModifyConversationNodeResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyConversationNodeHandler(
            IConfigurationRepository configurationRepository
        )
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyConversationNodeResponse> Handle(ModifyConversationNodeRequest request, CancellationToken cancellationToken)
        {
            var updatedConversation = await configurationRepository.UpdateConversationNode(request.IntentId, request.NodeId, request.NodeUpdate);
            await configurationRepository.CommitChangesAsync();
            return new ModifyConversationNodeResponse(updatedConversation);
        }
    }

    public class ModifyConversationNodeResponse
    {
        public ModifyConversationNodeResponse(List<ConversationNode> response) => Response = response;
        public List<ConversationNode> Response { get; set; }
    }

    public class ModifyConversationNodeRequest : IRequest<ModifyConversationNodeResponse>
    {
        public ModifyConversationNodeRequest(string nodeId, string intentId, ConversationNode nodeUpdate)
        {
            NodeId = nodeId;
            IntentId = intentId;
            NodeUpdate = nodeUpdate;
        }

        public string NodeId { get; set; }
        public string IntentId { get; set; }
        public ConversationNode NodeUpdate { get; set; }
    }
}