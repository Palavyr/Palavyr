using Palavyr.Core.Models.Configuration.Schemas;

namespace Test.Common.Builders.Conversations
{
    public class MultiNodeReturnObject : IHaveCurrentNode
    {
        public ConversationNode CurrentNode { get; set; }
        public ConversationNode[] ChildNodes { get; set; }

        private MultiNodeReturnObject(ConversationNode currentNode, ConversationNode[] node)
        {
            CurrentNode = currentNode;
            ChildNodes = node;
        }

        public static MultiNodeReturnObject Return(ConversationNode currentNode, params ConversationNode[] nodes)
        {
            return new MultiNodeReturnObject(currentNode, nodes);
        }
    }
}