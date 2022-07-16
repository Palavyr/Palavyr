using System.Collections.Generic;
using Palavyr.Core.Data.Entities;

namespace Test.Common.Builders.Conversations
{
    public static partial class BuilderExtensionMethods
    {
        public static ConversationBuilder CreateConversationBuilder(this IUnitTestFixture _)
        {
            return new ConversationBuilder();
        }
    }

    public class ConversationBuilder
    {
        
        private List<ConversationNode> conversation = new List<ConversationNode>();

        public ConversationNode WithNode(ConversationNode node)
        {
            conversation.Add(node);
            return node;
        }
        
        public ConversationNode WithNode(SingleNodeReturnObject nodeReturn)
        {
            conversation.Add(nodeReturn.PreviousNode);
            return nodeReturn.ChildNode;
        }

        public ConversationNode[] WithNode(MultiNodeReturnObject nodeReturn)
        {
            conversation.Add(nodeReturn.PreviousNode);
            return nodeReturn.ChildNodes;
        }

        public ConversationNode[] Build()
        {
            return conversation.ToArray();
        }
    }
}