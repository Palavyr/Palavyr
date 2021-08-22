using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models.Nodes
{
    public interface INodeBranchLengthCalculator
    {
        int GetLengthOfLongestTerminatingPath(ConversationNode[] nodeList, ConversationNode[] terminatingNodes);
    }

    public class NodeBranchLengthCalculator : INodeBranchLengthCalculator
    {
        private readonly INodeGetter nodeGetter;

        public NodeBranchLengthCalculator(
            INodeGetter nodeGetter
        )
        {
            this.nodeGetter = nodeGetter;
        }

        public int GetLengthOfLongestTerminatingPath(ConversationNode[] nodeList, ConversationNode[] terminatingNodes)
        {
            var listOfBranchLengths = new List<int>();
            foreach (var terminalNode in terminatingNodes)
            {
                var branchLength = ComputeBranchLength(terminalNode, nodeList);
                listOfBranchLengths.Add(branchLength);
            }

            return listOfBranchLengths.Max();
        }

        private int ComputeBranchLength(ConversationNode terminalNode, ConversationNode[] nodeList)
        {
            var branchNodeCount = 1; // the terminal node
            if (terminalNode.IsRoot) return branchNodeCount;
            var nextNode = terminalNode;
            while (!nextNode.IsRoot)
            {
                nextNode = nodeGetter.GetAnyParentNode(nodeList, terminalNode);
                branchNodeCount++;
                if (branchNodeCount > 200) break;
            }

            return branchNodeCount;
        }
    }
}