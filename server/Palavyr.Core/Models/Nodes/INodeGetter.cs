using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Models.Nodes
{
    public interface INodeGetter
    {
        ConversationNode GetAnyParentNode(ConversationNode[] nodeList, ConversationNode curNode);
        ConversationNode[] GetParentNodes(ConversationNode[] nodeList, ConversationNode[] curNodes);
    }

    public class NodeGetter : INodeGetter
    {
        private readonly IConversationOptionSplitter splitter;

        public NodeGetter(IConversationOptionSplitter splitter)
        {
            this.splitter = splitter;
        }

        public ConversationNode GetAnyParentNode(ConversationNode[] nodeList, ConversationNode curNode)
        {
            // does not consider merge-split or anabranch
            if (curNode.IsRoot)
            {
                return null;
            }

            var childId = curNode.NodeId;
            ConversationNode parent = null;
            foreach (var potentialParent in nodeList)
            {
                var childrenIds = potentialParent.NodeChildrenString!.Split(",").ToList();
                if (!childrenIds.Contains(childId)) continue;
                parent = potentialParent;
                break;
            }

            if (parent == null) throw new Exception();
            return parent;
        }
        
        public ConversationNode[] GetParentNodes(ConversationNode[] nodeList, ConversationNode[] curNodes)
        {
            if (curNodes.Length == 1 && curNodes.Single().IsRoot) return null;

            var childIds = curNodes.Select(x => x.NodeId).ToList();
            var parents = new List<ConversationNode>();
            foreach (var potentialParent in nodeList)
            {
                var childrenIds = splitter.SplitNodeChildrenString(potentialParent.NodeChildrenString);
                if (childrenIds.Intersect(childIds).ToList().Count == 0) continue;
                parents.Add(potentialParent);
            }

            return parents.ToArray();
        }
    }
}