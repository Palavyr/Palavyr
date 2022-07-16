using System.Collections.Generic;
using System.Linq;
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
    public class ModifyConversationNodeHandler : IRequestHandler<ModifyConversationNodeRequest, ModifyConversationNodeResponse>
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly IMapToNew<ConversationNode, ConversationDesignerNodeResource> mapper;

        public ModifyConversationNodeHandler(IEntityStore<Intent> intentStore, IMapToNew<ConversationNode, ConversationDesignerNodeResource> mapper)
        {
            this.intentStore = intentStore;
            this.mapper = mapper;
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
                newNode.IntentId,
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


            var resource = await mapper.MapMany(convoUpdate, cancellationToken);
            return new ModifyConversationNodeResponse(resource);
        }
    }

    public class ModifyConversationNodeResponse
    {
        public ModifyConversationNodeResponse(IEnumerable<ConversationDesignerNodeResource> response) => Response = response;
        public IEnumerable<ConversationDesignerNodeResource> Response { get; set; }
    }

    public class ModifyConversationNodeRequest : IRequest<ModifyConversationNodeResponse>
    {
        public ModifyConversationNodeRequest(string nodeId, string intentId, ConversationDesignerNodeResource nodeUpdate)
        {
            NodeId = nodeId;
            IntentId = intentId;
            NodeUpdate = nodeUpdate;
        }

        public string NodeId { get; set; }
        public string IntentId { get; set; }
        public ConversationDesignerNodeResource NodeUpdate { get; set; }
    }
}