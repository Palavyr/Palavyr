using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.Domain
{
    public static class TreeUtils
    {
        public static string TransformRequiredNodeType(DynamicTableMeta dynamicTableMeta)
        {
            return string.Join("-", new[] {dynamicTableMeta.TableType, dynamicTableMeta.TableId});
        }

        public static string TransformRequiredNodeTypeToPrettyName(DynamicTableMeta dynamicTableMeta)
        {
            return string.Join("-", new[] {dynamicTableMeta.PrettyName, dynamicTableMeta.TableTag});
        }

        public static int TraverseTheTreeFromTheTop(ConversationNode[] nodeList, ConversationNode node)
        {
            var count = 0;
            if (node.IsTerminalType)
            {
                return 1;
            }

            var children = node.NodeChildrenString.Split(",");
            foreach (var child in children)
            {
                var childNode = nodeList.Where(row => row.NodeId == child).ToArray();
                if (childNode.Length > 0)
                {
                    count += TraverseTheTreeFromTheTop(nodeList, childNode.First());
                }
            }

            return count;
        }

        public static ConversationNode GetRootNode(ConversationNode[] nodeList)
        {
            return nodeList.Single(row => row.IsRoot == true);
        }

        public static int GetNumTerminal(ConversationNode[] nodeList)
        {
            return nodeList.Count(node => node.IsTerminalType);
        }

        public static ConversationNode GetParentNode(ConversationNode[] nodeList, ConversationNode curNode)
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
        public static string[] TraverseTheTreeFromBottom(
            ConversationNode node,
            ConversationNode[] nodeList,
            string[] requiredNodes // array of node type names
        )
        {
            if (node.IsRoot)
            {
                return requiredNodes;
            }

            var requiredNodesClone = new List<string>(requiredNodes);
            if (requiredNodesClone.Contains(node.NodeType))
            {
                requiredNodesClone.RemoveAt(requiredNodesClone.FindIndex(x => x == node.NodeType));
            }

            var nextNode = GetParentNode(nodeList, node);
            return TraverseTheTreeFromBottom(nextNode, nodeList, requiredNodesClone.ToArray());
        }

        public static string[] GetMissingNodes(ConversationNode[] nodeList, string[] requiredNodes)
        {
            var allMissingNodeTypes = new List<string>();
            var terminalNodes = GetCompletePathTerminalNodes(nodeList);

            foreach (var terminalNode in terminalNodes)
            {
                var missingNodes = TraverseTheTreeFromBottom(terminalNode, nodeList, requiredNodes);
                allMissingNodeTypes.AddRange(missingNodes);
            }

            return allMissingNodeTypes.ToArray();
        }

        // Complete path terminal nodes are those that result in a response being sent. (dynamic and per individual counts)
        public static ConversationNode[] GetCompletePathTerminalNodes(ConversationNode[] nodeList)
        {
            return nodeList
                .Where(node => node.IsTerminalType && node.NodeType != DefaultNodeTypeOptions.TooComplicated.StringName)
                .ToArray();
        }

        public static string CreateNodeChildrenString(params string[] nodeIds)
        {
            return string.Join(Delimiters.NodeChildrenStringDelimiter, nodeIds);
        }

        public static string CreateValueOptions(params string[] options)
        {
            return string.Join(Delimiters.PathOptionDelimiter, options);
        }
    }
}