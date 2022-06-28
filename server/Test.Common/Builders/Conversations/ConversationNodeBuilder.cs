using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Schemas;
using Test.Common.Constants;

namespace Test.Common.Builders.Conversations
{
    public static partial class BuilderExtensionMethods
    {
        public static ConversationNodeBuilder CreateConversationNodeBuilder(this IUnitTestFixture _)
        {
            return new ConversationNodeBuilder();
        }
    }

    public class ConversationNodeBuilder
    {
        public ConversationNode CreateRootNode(string accountId = DefaultConstants.AccountId, string areaIdentifier = DefaultConstants.AreaIdentifier)
        {
            return new ConversationNode
            {
                IsRoot = true,
                NodeId = StaticGuidUtils.CreateNewId(),
                Text = "Root Node",
                AreaIdentifier = areaIdentifier,
                AccountId = accountId,
            };
        }
    }
}