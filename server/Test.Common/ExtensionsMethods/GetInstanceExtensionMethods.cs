using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Services.DynamicTableService.Compilers;

namespace Test.Common.ExtensionsMethods
{
    public static class GetInstanceExtensionMethods
    {
        public static IConversationOptionSplitter GetNodeSplitter(this ITestBase _)
        {
            return new ConversationOptionSplitter(new GuidFinder());
        }

        public static NodeGetter GetNodeGetter(this ITestBase _)
        {
            return new NodeGetter(new ConversationOptionSplitter(new GuidFinder()));
        }
    }
}