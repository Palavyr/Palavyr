using Palavyr.Core.Models.Configuration.Schemas;

namespace Test.Common.Builders.Conversations
{
    public class MultiNodeReturnObject : IHaveCurrentNode
    {
        public ConversationNode PreviousNode { get; set; }
        public ConversationNode[] ChildNodes { get; set; }

        private MultiNodeReturnObject(ConversationNode previousNode, ConversationNode[] node)
        {
            PreviousNode = previousNode;
            ChildNodes = node;
        }

        public static MultiNodeReturnObject Return(ConversationNode previousNode, params ConversationNode[] nextNodes)
        {
            return new MultiNodeReturnObject(previousNode, nextNodes);
        }
    }
}