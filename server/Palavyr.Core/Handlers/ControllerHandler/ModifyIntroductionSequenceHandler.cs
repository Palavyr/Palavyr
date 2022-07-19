using System.Collections.Generic;
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
    public class ModifyIntroductionSequenceHandler : IRequestHandler<ModifyIntroductionSequenceRequest, ModifyIntroductionSequenceResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<Account> accountStore;
        private readonly IMapToNew<ConversationNode, ConversationDesignerNodeResource> conversationDesignerNodeResourceMapper;
        private readonly IMapToNew<ConversationDesignerNodeResource, ConversationNode> conversationNodeMapper;

        public ModifyIntroductionSequenceHandler(
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<Account> accountStore,
            IMapToNew<ConversationNode, ConversationDesignerNodeResource> conversationDesignerNodeResourceMapper,
            IMapToNew<ConversationDesignerNodeResource, ConversationNode> conversationNodeMapper)
        {
            this.convoNodeStore = convoNodeStore;
            this.accountStore = accountStore;
            this.conversationDesignerNodeResourceMapper = conversationDesignerNodeResourceMapper;
            this.conversationNodeMapper = conversationNodeMapper;
        }

        public async Task<ModifyIntroductionSequenceResponse> Handle(ModifyIntroductionSequenceRequest request, CancellationToken cancellationToken)
        {
            var conversationNodes = await conversationNodeMapper
                .MapMany(request.Transactions, cancellationToken);

            var account = await accountStore.GetAccount();
            await convoNodeStore.Delete(account.IntroIntentId, s => s.IntentId);

            await convoNodeStore.CreateMany(conversationNodes);

            // var resources = await conversationDesignerNodeResourceMapper.MapMany()
            return new ModifyIntroductionSequenceResponse(request.Transactions.ToArray());
        }
    }

    public class ModifyIntroductionSequenceResponse
    {
        public ModifyIntroductionSequenceResponse(ConversationDesignerNodeResource[] response) => Response = response;
        public ConversationDesignerNodeResource[] Response { get; set; }
    }

    public class ModifyIntroductionSequenceRequest : IRequest<ModifyIntroductionSequenceResponse>
    {
        public List<ConversationDesignerNodeResource> Transactions { get; set; }
    }


    public class ConversationNodeMapper : IMapToNew<ConversationDesignerNodeResource, ConversationNode>
    {
        private readonly IEntityStore<Account> accountStore;

        public ConversationNodeMapper(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<ConversationNode> Map(ConversationDesignerNodeResource from, CancellationToken cancellationToken = default)
        {
            var account = await accountStore.GetAccount();

            return new ConversationNode
            {
                NodeId = from.NodeId,
                NodeType = from.NodeType,
                Text = from.Text,
                IsRoot = from.IsRoot,
                IntentId = from.IntentId,
                OptionPath = from.OptionPath,
                NodeChildrenString = from.NodeChildrenString,
                ValueOptions = from.ValueOptions,
                IsCritical = from.IsCritical,
                AccountId = account.AccountId,
                IsMultiOptionType = from.IsMultiOptionEditable,
                IsTerminalType = from.IsTerminalType,
                ShouldRenderChildren = from.ShouldRenderChildren,
                ShouldShowMultiOption = from.ShouldShowMultiOption,
                IsAnabranchType = from.IsAnabranchType,
                IsAnabranchMergePoint = from.IsAnabranchMergePoint,
                IsPricingStrategyTableNode = from.IsPricingStrategyNode,
                IsCurrency = from.IsCurrency,
                IsMultiOptionEditable = from.IsMultiOptionEditable,
                PricingStrategyType = from.PricingStrategyType,
                IsImageNode = from.IsImageNode,
                FileId = from.FileId,
                IsLoopbackAnchorType = from.IsLoopbackAnchorType,
                NodeComponentType = from.NodeComponentType,
                ResolveOrder = from.ResolveOrder,
                NodeTypeCode = from.NodeTypeCode
            };
        }
    }
}