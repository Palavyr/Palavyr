using Palavyr.Core.Data.Entities;

namespace Test.Common.Builders.Conversations
{
    public class SingleNodeReturnObject : IHaveCurrentNode
    {
        public ConversationNode PreviousNode { get; set; }
        public ConversationNode ChildNode { get; set; }

        private SingleNodeReturnObject(ConversationNode currentNode, ConversationNode node)
        {
            PreviousNode = currentNode;
            ChildNode = node;
        }

        public static SingleNodeReturnObject Return(ConversationNode previousNode, ConversationNode nextNode)
        {
            return new SingleNodeReturnObject(previousNode, nextNode);
        }
    }
}