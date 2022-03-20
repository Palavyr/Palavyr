using System.Linq;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.DynamicTableService;

namespace Test.Common.Builders.Conversations.NodeAdders
{
    public static class ConvoBuilderExtensionMethod
    {
        public static ConversationNode AttachNewChildId(this ConversationNode node, string newId)
        {
            var splitter = new ConversationOptionSplitter(new GuidFinder());

            var previousNodeChildren = splitter.SplitNodeChildrenString(node.NodeChildrenString).ToList();
            previousNodeChildren.Add(newId);
            node.NodeChildrenString = splitter.JoinNodeChildrenArray(previousNodeChildren);
            return node;
        }
    }
}