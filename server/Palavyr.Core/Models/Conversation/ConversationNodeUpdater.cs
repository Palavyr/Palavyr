using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Models.Conversation
{
    public class ConversationNodeUpdater : IConversationNodeUpdater
    {
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IEntityStore<Area> intentStore;
        private readonly IOrphanRemover orphanRemover;

        public ConversationNodeUpdater(
            IAccountIdTransport accountIdTransport,
            IEntityStore<Area> intentStore,
            IOrphanRemover orphanRemover
        )
        {
            this.accountIdTransport = accountIdTransport;
            this.intentStore = intentStore;
            this.orphanRemover = orphanRemover;
        }

        public async Task<List<ConversationNode>> UpdateConversation(string intentId, List<ConversationNode> updatedConvo, CancellationToken cancellationToken)
        {
            var mappedUpdates = MapUpdate(updatedConvo);
            var deOrphanedAreaConvo = orphanRemover.RemoveOrphanedNodes(mappedUpdates);

            var intent = await intentStore.GetIntentComplete(intentId);
            intent.ConversationNodes.Clear();
            intent.ConversationNodes.AddRange(deOrphanedAreaConvo);

            return deOrphanedAreaConvo;
        }

        private List<ConversationNode> MapUpdate(List<ConversationNode> nodeUpdates)
        {
            var mappedTransactions = new List<ConversationNode>();
            foreach (var node in nodeUpdates)
            {
                var mappedNode = ConversationNode.CreateNew(
                    node.NodeId,
                    node.NodeType,
                    node.Text,
                    node.AreaIdentifier,
                    node.NodeChildrenString,
                    node.OptionPath,
                    node.ValueOptions,
                    accountIdTransport.AccountId,
                    node.NodeComponentType,
                    node.NodeTypeCode,
                    node.IsRoot,
                    node.IsCritical,
                    node.IsMultiOptionType,
                    node.IsTerminalType,
                    node.ShouldRenderChildren,
                    node.ShouldShowMultiOption,
                    node.IsAnabranchType,
                    node.IsAnabranchMergePoint,
                    node.IsDynamicTableNode,
                    node.IsCurrency,
                    node.IsMultiOptionEditable,
                    node.ResolveOrder,
                    node.DynamicType,
                    node.IsImageNode,
                    node.ImageId,
                    node.IsLoopbackAnchorType
                );
                mappedTransactions.Add(mappedNode);
            }

            return mappedTransactions;
        }
    }
}