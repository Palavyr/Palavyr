using Palavyr.Core.Models.Configuration.Schemas;

namespace Test.Common.Builders.Conversations
{
    public interface IHaveCurrentNode
    {
        public ConversationNode PreviousNode { get; set; }
    }
}