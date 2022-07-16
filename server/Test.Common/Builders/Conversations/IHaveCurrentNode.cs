using Palavyr.Core.Data.Entities;

namespace Test.Common.Builders.Conversations
{
    public interface IHaveCurrentNode
    {
        public ConversationNode PreviousNode { get; set; }
    }
}