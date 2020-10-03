using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.API.ResponseTypes;
using Server.Domain.Configuration.constants;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{

    /// <summary>
    /// Main method
    /// </summary>
    public static class PreCheckUtils
    {
        public static PreCheckResult RunConversationsPreCheck(List<Area> areas)
        {
            var incompleteAreas = new List<Area>();

            var isReady = true;
            foreach (var area in areas)
            {
                var nodeList = area.ConversationNodes;
                var requiredNodes = area.DynamicTableMetas.Select(row => row.TableType).ToList();
                var checks = new List<bool>()
                {
                    AllNodesAreSet(nodeList),
                    AllBranchesTerminate(nodeList),
                    AllRequiredNodesSatisfied(nodeList, requiredNodes),
                };

                isReady = checks.TrueForAll(x => x == true);

                if (isReady) continue;
                incompleteAreas.Add(area);
                Console.WriteLine($"Area not currently ready: {area.AreaName}");

            }
            return PreCheckResult.CreateConvoResult(incompleteAreas, isReady);
        }


        private static bool AllNodesAreSet(List<ConversationNode> nodeList)
        {
            var EmptyNodeTypes = nodeList.Select(x => string.IsNullOrEmpty(x.NodeType)).ToList();
            return EmptyNodeTypes.All(x => x == false);
        }

        private static bool AllBranchesTerminate(List<ConversationNode> nodeList)
        { 
            var rootNode = GetRootNode(nodeList);
            var numLeaves = TraverseTheTreeFromTheTop(nodeList, rootNode);
            var numTerminal = GetNumTerminal(nodeList);
            return (numLeaves == numTerminal);
        }

        private static bool AllRequiredNodesSatisfied(List<ConversationNode> nodeList, List<string> requiredNodes)
        {
            var missingNodes = GetMissingNodes(nodeList, requiredNodes);
            return missingNodes.Count == 0;
        }

        private static int TraverseTheTreeFromTheTop(List<ConversationNode> nodeList, ConversationNode node)
        {
            var count = 0;
            if (node.NodeType == NodeTypes.TooComplicated || node.NodeType == NodeTypes.EndingSequence)
            {
                return count + 1;
            }

            var children = node.NodeChildrenString.Split(",");
            foreach (var child in children)
            {
                var childNode = nodeList.Where(row => row.NodeId == child).ToArray();
                if (childNode.Length > 0)
                {
                    count += TraverseTheTreeFromTheTop(nodeList, childNode[0]);
                }
            }

            return count;
        }

        private static ConversationNode GetRootNode(List<ConversationNode> nodeList)
        {
            return nodeList.Single(row => row.IsRoot == true);
        }

        private static int GetNumTerminal(List<ConversationNode> nodeList)
        {
            return nodeList.Count(node => node.NodeType == NodeTypes.TooComplicated || node.NodeType == NodeTypes.EndingSequence);
        }

        private static ConversationNode GetParentNode(List<ConversationNode> nodeList, ConversationNode curNode)
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
        
        private static List<string> TraverseTheTreeFromBottom(ConversationNode node, List<ConversationNode> nodeList,
            List<string> requiredNodes)
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
            return TraverseTheTreeFromBottom(nextNode, nodeList, requiredNodesClone);
        }

        private static List<string> GetMissingNodes(List<ConversationNode> nodeList, List<string> requiredNodes)
        {
            var allMissingNodeTypes = new List<string>();
            var terminalNodes = GetEndingSequenceNodes(nodeList);

            foreach (var terminalNode in terminalNodes)
            {
                var missingNodes = TraverseTheTreeFromBottom(terminalNode, nodeList, requiredNodes);
                allMissingNodeTypes.AddRange(missingNodes);
            }

            return allMissingNodeTypes;
        }

        private static List<ConversationNode> GetEndingSequenceNodes(List<ConversationNode> nodeList)
        {
            return nodeList.Where(node => node.NodeType == NodeTypes.EndingSequence).ToList();
        }
    }
}