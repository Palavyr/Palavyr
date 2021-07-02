using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Models.Conversation
{
    public class ConversationNodeUpdater : IConversationNodeUpdater
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly OrphanRemover orphanRemover;

        public ConversationNodeUpdater(
            IConfigurationRepository configurationRepository,
            OrphanRemover orphanRemover
        )
        {
            this.configurationRepository = configurationRepository;
            this.orphanRemover = orphanRemover;
        }

        public async Task<List<ConversationNode>> UpdateConversation(string accountId, string areaId, List<ConversationNode> updatedConvo, CancellationToken cancellationToken)
        {
            var mappedUpdates = MapUpdate(accountId, updatedConvo);
            var deOrphanedAreaConvo = orphanRemover.RemoveOrphanedNodes(mappedUpdates);
            var updated = await configurationRepository.UpdateConversation(accountId, areaId, deOrphanedAreaConvo, cancellationToken);
            return updated;
        }

        private List<ConversationNode> MapUpdate(string accountId, List<ConversationNode> nodeUpdates)
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
                    accountId,
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