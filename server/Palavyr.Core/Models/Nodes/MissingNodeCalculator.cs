using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models.Nodes
{
    public class MissingNodeCalculator
    {
        private readonly INodeGetter nodeGetter;

        public MissingNodeCalculator(INodeGetter nodeGetter)
        {
            this.nodeGetter = nodeGetter;
        }

        public string[] CalculateMissingNodes(
            NodeTypeOption[] requiredDynamicNodeTypes,
            List<ConversationNode> conversationNodes,
            List<DynamicTableMeta> dynamicTableMetas,
            List<StaticTablesMeta> staticTablesMetas)
        {
            var allMissingNodeTypes = new List<string>();

            if (requiredDynamicNodeTypes.Length > 0)
            {
                var rawMissingDynamicNodeTypes = FindMissingNodes(conversationNodes.ToArray(), requiredDynamicNodeTypes);
                var names = rawMissingDynamicNodeTypes.Select(x => x.Text).ToList();
                allMissingNodeTypes.AddRange(names);
            }

            return allMissingNodeTypes.ToArray();
        }

        public NodeTypeOption[] FindMissingNodes(ConversationNode[] nodeList, NodeTypeOption[] requiredNodes)
        {
            var allMissingNodeTypes = new List<NodeTypeOption>();
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

        public NodeTypeOption[] SearchTerminalResponseBranchesForMissingRequiredNodes(
            ConversationNode node,
            ConversationNode[] nodeList,
            NodeTypeOption[] requiredNodes // array of node type names
        )
        {
            var requiredNodesClone = new List<NodeTypeOption>(requiredNodes);
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