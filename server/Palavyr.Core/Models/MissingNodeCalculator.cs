using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.Core.Models
{
    public class MissingNodeCalculator
    {
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
                .Where(node => node.IsTerminalType && node.NodeType != DefaultNodeTypeOptions.TooComplicated.StringName)
                .ToArray();
        }

        ConversationNode GetParentNode(ConversationNode[] nodeList, ConversationNode curNode)
        {
            var childId = curNode.NodeId;
            ConversationNode parent = null;
            foreach (var potentialParent in nodeList)
            {
                var childrenIds = potentialParent.NodeChildrenString.Split(",").ToList();
                if (!childrenIds.Contains(childId)) continue;
                parent = potentialParent;
                break;
            }

            if (parent == null) throw new Exception();
            return parent;
        }

        public NodeTypeOption[] SearchTerminalResponseBranchesForMissingRequiredNodes(
            ConversationNode node,
            ConversationNode[] nodeList,
            NodeTypeOption[] requiredNodes // array of node type names
        )
        {
            var requiredNodesClone = new List<NodeTypeOption>(requiredNodes);
            if (requiredNodesClone.Select(x => x.Value.TrimLastGuidChunk()).Contains(node.NodeType.TrimLastGuidChunk()))
            {
                requiredNodesClone.RemoveAt(requiredNodesClone.Select(x => x.Value.TrimLastGuidChunk()).ToList().FindIndex(x => x == node.NodeType.TrimLastGuidChunk()));
            }

            if (node.IsRoot)
            {
                return requiredNodesClone.ToArray();
            }

            var nextNode = GetParentNode(nodeList, node);
            return SearchTerminalResponseBranchesForMissingRequiredNodes(nextNode, nodeList, requiredNodesClone.ToArray());
        }
    }
}