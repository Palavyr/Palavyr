using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Models.Nodes
{
    public interface IMissingNodeCalculator
    {
        string[] CalculateMissingNodes(
            NodeTypeOptionResource[] requiredPricingStrategyNodeTypes,
            List<ConversationNode> conversationNodes,
            List<PricingStrategyTableMeta> pricingStrategyTableMetas,
            List<StaticTablesMeta> staticTablesMetas);

        NodeTypeOptionResource[] FindMissingNodes(ConversationNode[] nodeList, NodeTypeOptionResource[] requiredNodes);

        NodeTypeOptionResource[] SearchTerminalResponseBranchesForMissingRequiredNodes(
            ConversationNode node,
            ConversationNode[] nodeList,
            NodeTypeOptionResource[] requiredNodes // array of node type names
        );
    }

    public class MissingNodeCalculator : IMissingNodeCalculator
    {
        private readonly INodeGetter nodeGetter;

        public MissingNodeCalculator(INodeGetter nodeGetter)
        {
            this.nodeGetter = nodeGetter;
        }

        public string[] CalculateMissingNodes(
            NodeTypeOptionResource[] requiredPricingStrategyNodeTypes,
            List<ConversationNode> conversationNodes,
            List<PricingStrategyTableMeta> pricingStrategyTableMetas,
            List<StaticTablesMeta> staticTablesMetas)
        {
            var allMissingNodeTypes = new List<string>();

            if (requiredPricingStrategyNodeTypes.Length > 0)
            {
                var rawMissingPricingStrategyNodeTypes = FindMissingNodes(conversationNodes.ToArray(), requiredPricingStrategyNodeTypes);
                var names = rawMissingPricingStrategyNodeTypes.Select(x => x.Text).ToList();
                allMissingNodeTypes.AddRange(names);
            }

            return allMissingNodeTypes.ToArray();
        }

        public NodeTypeOptionResource[] FindMissingNodes(ConversationNode[] nodeList, NodeTypeOptionResource[] requiredNodes)
        {
            var allMissingNodeTypes = new List<NodeTypeOptionResource>();
            var terminalNodes = GetCompletePathTerminalNodes(nodeList);

            foreach (var terminalNode in terminalNodes)
            {
                var missingNodes = SearchTerminalResponseBranchesForMissingRequiredNodes(terminalNode, nodeList, requiredNodes);
                allMissingNodeTypes.AddRange(missingNodes);
            }

            return allMissingNodeTypes.ToArray();
        }

        ConversationNode[] GetCompletePathTerminalNodes(ConversationNode[] nodeList)
        {
            return nodeList
                .Where(
                    node => node.IsTerminalType
                            && node.NodeType != DefaultNodeTypeOptions.TooComplicated.StringName
                            && node.NodeType != DefaultNodeTypeOptions.EndWithoutEmail.StringName)
                .ToArray();
        }

        public NodeTypeOptionResource[] SearchTerminalResponseBranchesForMissingRequiredNodes(
            ConversationNode node,
            ConversationNode[] nodeList,
            NodeTypeOptionResource[] requiredNodes // array of node type names
        )
        {
            var requiredNodesClone = new List<NodeTypeOptionResource>(requiredNodes);
            if (requiredNodesClone.Select(x => x.Value).Contains(node.NodeType))
            {
                requiredNodesClone.RemoveAt(requiredNodesClone.Select(x => x.Value).ToList().FindIndex(x => x == node.NodeType));
            }

            if (requiredNodesClone.Count == 0)
            {
                return requiredNodesClone.ToArray();
            }

            if (node.IsRoot)
            {
                return requiredNodesClone.ToArray();
            }

            var nextNode = nodeGetter.GetAnyParentNode(nodeList, node);
            return SearchTerminalResponseBranchesForMissingRequiredNodes(nextNode, nodeList, requiredNodesClone.ToArray());
        }
    }
}