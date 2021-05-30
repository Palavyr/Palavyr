using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.Core.Models
{
    public class TreeWalker
    {
        private readonly IConversationOptionSplitter splitter;

        public TreeWalker(IConversationOptionSplitter splitter)
        {
            this.splitter = splitter;
        }
        public int CountAllTerminalNodes(ConversationNode[] nodeList, ConversationNode node, int count)
        {
            if (node.IsTerminalType)
            {
                return 1;
            }

            var children = splitter.SplitNodeChildrenString(node.NodeChildrenString);
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
    }
}