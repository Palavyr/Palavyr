using System.Linq;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.DynamicTableService.Compilers;

namespace Test.Common.Builders.Conversations.NodeAdders
{
    public static class ConvoBuilderExtensionMethod
    {
        public static ConversationNode AttachNewChildId(this ConversationNode conversationNode, string newId)
        {
            var splitter = new ConversationOptionSplitter(new GuidFinder());

            var previousNodeChildren = splitter.SplitNodeChildrenString(conversationNode.NodeChildrenString).ToList();
            previousNodeChildren.Add(newId);
            conversationNode.NodeChildrenString = splitter.JoinNodeChildrenArray(previousNodeChildren);
            return conversationNode;
        }
    }
}