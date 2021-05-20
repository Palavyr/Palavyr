using Palavyr.Core.Models.Configuration.Schemas;

namespace Test.Common.Builders.Conversations
{
    public class SingleNodeReturnObject : IHaveCurrentNode
    {
        public ConversationNode CurrentNode { get; set; }
        public ConversationNode ChildNode { get; set; }

        private SingleNodeReturnObject(ConversationNode currentNode, ConversationNode node)
        {
            CurrentNode = currentNode;
            ChildNode = node;
        }

        public static SingleNodeReturnObject Return(ConversationNode currentNode, ConversationNode nextNode)
        {
            return new SingleNodeReturnObject(currentNode, nextNode);
        }
    }
}