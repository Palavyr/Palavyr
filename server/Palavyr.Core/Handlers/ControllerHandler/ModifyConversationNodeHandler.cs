using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyConversationNodeHandler : IRequestHandler<ModifyConversationNodeRequest, ModifyConversationNodeResponse>
    {
        private readonly IConfigurationEntityStore<Area> intentStore;

        public ModifyConversationNodeHandler(IConfigurationEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyConversationNodeResponse> Handle(ModifyConversationNodeRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.GetIntentComplete(request.IntentId);
            var convoUpdate = intent.ConversationNodes.Where(n => n.NodeId != request.NodeId).ToList();

            var newNode = request.NodeUpdate;
            var updatedNode = ConversationNode.CreateNew(
                newNode.NodeId,
                newNode.NodeType,
                newNode.Text,
                newNode.AreaIdentifier,
                newNode.NodeChildrenString,
                newNode.OptionPath,
                newNode.ValueOptions,
                intentStore.AccountId,
                newNode.NodeComponentType,
                newNode.NodeTypeCode,
                newNode.IsRoot,
                newNode.IsCritical,
                newNode.IsMultiOptionType,
                newNode.IsTerminalType
            );

            convoUpdate.Add(updatedNode);
            intent.ConversationNodes = convoUpdate;
            return new ModifyConversationNodeResponse(convoUpdate);
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