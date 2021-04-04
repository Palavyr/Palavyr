using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Common.Utils;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models
{
    public class MissingNodeCalculator
    {
        public string[] CalculateMissingNodes(NodeTypeOption[] requiredDynamicNodeTypes, List<ConversationNode> conversationNodes, List<DynamicTableMeta> dynamicTableMetas, List<StaticTablesMeta> staticTablesMetas)
        {
            var allMissingNodeTypes = new List<string>();

            if (requiredDynamicNodeTypes.Length > 0)
            {
                var rawMissingDynamicNodeTypes = FindMissingNodes(conversationNodes.ToArray(), requiredDynamicNodeTypes);
                var missingDynamicNodeTypes = dynamicTableMetas
                    .Where(x => rawMissingDynamicNodeTypes.Select(x => x.Value).Contains(TreeUtils.TransformRequiredNodeType(x)))
                    .Select(TreeUtils.TransformRequiredNodeTypeToPrettyName)
                    .ToList();

                allMissingNodeTypes.AddRange(missingDynamicNodeTypes);
            }

            var perIndividualRequiredStaticTables = staticTablesMetas
                .Select(p => p.PerPersonInputRequired)
                .Any(r => r);

            if (perIndividualRequiredStaticTables && !allMissingNodeTypes.Contains(DefaultNodeTypeOptions.TakeNumberIndividuals.StringName))
            {
                var perPersonNodeType = new NodeTypeOption[] {DefaultNodeTypeOptions.CreateTakeNumberIndividuals()};
                var missingOtherNodeTypes = FindMissingNodes(conversationNodes.ToArray(), perPersonNodeType);
                if (missingOtherNodeTypes.Length > 0)
                {
                    var asPretty = string.Join(" ", StringUtils.SplitCamelCaseAsString(missingOtherNodeTypes.Select(x => x.Value).Single()));
                    allMissingNodeTypes.Add(asPretty);
                }
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

        // TODO: Refactor to return the nodes. We only count the list in the widget status utils.
        public NodeTypeOption[] SearchTerminalResponseBranchesForMissingRequiredNodes(
            ConversationNode node,
            ConversationNode[] nodeList,
            NodeTypeOption[] requiredNodes // array of node type names
        )
        {
            if (node.IsRoot)
            {
                return requiredNodes;
            }

            var requiredNodesClone = new List<NodeTypeOption>(requiredNodes);
            if (requiredNodesClone.Select(x => x.Value).Contains(node.NodeType))
            {
                requiredNodesClone.RemoveAt(requiredNodesClone.Select(x => x.Value).ToList().FindIndex(x => x == node.NodeType));
            }

            var nextNode = GetParentNode(nodeList, node);
            return SearchTerminalResponseBranchesForMissingRequiredNodes(nextNode, nodeList, requiredNodesClone.ToArray());
        }
    }
}