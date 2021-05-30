using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Test.Common.Constants;

namespace Test.Common.Builders.Conversations.NodeAdders
{
    public static class AddProvideInfoExtensionMethod
    {
        public static SingleNodeReturnObject AddProvideInfo(this ConversationNode previousNode, string areaId = DefaultConstants.AreaIdentifier, string accountId = DefaultConstants.AccountId)
        {
            var thisNodesId = StaticGuidUtils.CreateNewId();
            previousNode.AttachNewChildId(thisNodesId);

            var provideInfo = ConversationNode.CreateNew(
                thisNodesId,
                DefaultNodeTypeOptions.ProvideInfo.StringName,
                "",
                areaId,
                "",
                "",
                "",
                accountId,
                DefaultNodeTypeOptions.ProvideInfo.StringName,
                false,
                false,
                false,
                false
            );

            return SingleNodeReturnObject.Return(previousNode, provideInfo);
        }
    }
}