using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    
    public class ConversationNodeResourceMapper : IMapToNew<ConversationNode, ConversationDesignerNodeResource>
    {
        public async Task<ConversationDesignerNodeResource> Map(ConversationNode @from)
        {
            await Task.CompletedTask;
            return new ConversationDesignerNodeResource()
            {
                NodeId = @from.NodeId,
                NodeType = @from.NodeType,
                Text = @from.Text,
                IsRoot = @from.IsRoot,
                AreaIdentifier = @from.AreaIdentifier,
                NodeChildrenString = @from.NodeChildrenString,
                OptionPath = @from.OptionPath,
                ValueOptions = @from.ValueOptions,
                IsCritical = @from.IsCritical,
                AccountId = @from.AccountId,
                IsMultiOptionType = @from.IsMultiOptionType,
                IsTerminalType = @from.IsTerminalType,
                ShouldRenderChildren = @from.ShouldRenderChildren,
                ShouldShowMultiOption = @from.ShouldShowMultiOption,
                IsAnabranchType = @from.IsAnabranchType,
                IsAnabranchMergePoint = @from.IsAnabranchMergePoint,
                IsCurrency = @from.IsCurrency,
                IsMultiOptionEditable = @from.IsMultiOptionEditable,
                IsDynamicTableNode = @from.IsDynamicTableNode,
                ResolveOrder = @from.ResolveOrder,
                NodeComponentType = @from.NodeComponentType,
                DynamicType = @from.DynamicType,
                IsImageNode = @from.IsImageNode,
                ImageId = @from.ImageId,
                IsLoopbackAnchorType = @from.IsLoopbackAnchorType,
                NodeTypeCode = @from.NodeTypeCode
            };
        }
    }

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
            var resource = await mapper.Map(updatedConversationNode);
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