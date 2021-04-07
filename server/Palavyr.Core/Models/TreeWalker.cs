using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models
{
    public class TreeWalker
    {
        public int CountAllTerminalNodes(ConversationNode[] nodeList, ConversationNode node, int count)
        {
            if (node.IsTerminalType)
            {
                return 1;
            }

            var children = node.NodeChildrenString.Split(",");
            foreach (var child in children)
            {
                var childNode = nodeList.SingleOrDefault(row => row.NodeId == child);
                if (childNode != null)
                {
                    count += CountAllTerminalNodes(nodeList, childNode, count);
                }
            }

            return count;
        }

        public List<ConversationNode> FindAllTerminalNodes(ConversationNode[] nodeList, ConversationNode node, List<ConversationNode> foundNodes)
        {
            if (node.IsTerminalType)
            {
                return new List<ConversationNode>(){node};
            }

            var children = node.NodeChildrenString.Split(",");
            foreach (var child in children)
            {
                var childNode = nodeList.SingleOrDefault(row => row.NodeId == child);
                if (childNode != null)
                {
                    foundNodes.AddRange(FindAllTerminalNodes(nodeList, childNode, foundNodes));
                }
            }
            return foundNodes;
        }
    }
}