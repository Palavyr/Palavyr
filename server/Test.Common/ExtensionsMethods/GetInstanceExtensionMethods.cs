using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Services.DynamicTableService;

namespace Test.Common.ExtensionsMethods
{
    public static class GetInstanceExtensionMethods
    {
        public static IConversationOptionSplitter GetNodeSplitter(this ITestBase _)
        {
            return new ConversationOptionSplitter(new GuidFinder());
        }

        public static INodeGetter GetNodeGetter(this ITestBase _)
        {
            return new INodeGetter(new ConversationOptionSplitter(new GuidFinder()));
        }

        public static GuidFinder GetGuidFinder(this ITestBase _)
        {
            return new GuidFinder();
        }
    }
}