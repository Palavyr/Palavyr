using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.Core.Models
{
    public interface ITreeWalker
    {
        int CountAllTerminalNodes(ConversationNode[] nodeList, ConversationNode node, int count);
    }

    public class TreeWalker : ITreeWalker
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

        // public int GetLengthOfLongestTerminatingPath(ConversationNode[] nodeList, ConversationNode[] terminatingNodes)
        // {
        //     var listOfBranchLenghts = new List<int>();
        //     foreach (var terminalNode in terminatingNodes)
        //     {
        //         var branchLength = ComputeBranchLength(terminalNode, nodeList);
        //     }
        //
        //     return listOfBranchLenghts.Max();
        // }
        //
        // private int ComputeBranchLength(ConversationNode terminalNode, ConversationNode[] nodeList)
        // {
        //     
        // }
    }
}