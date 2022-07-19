using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Mappers
{
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
                NodeTypeCodeEnum = from.NodeTypeCodeEnum
            };
        }
    }
}