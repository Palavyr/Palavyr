using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Configuration.Constant;
using Test.Common.Constants;

namespace Test.Common.Builders.Conversations.NodeAdders
{
    public static class AddProvideInfoExtensionMethod
    {
        public static SingleNodeReturnObject AddProvideInfo(this ConversationNode previousNode, string intentId = DefaultConstants.IntentId, string accountId = DefaultConstants.AccountId)
        {
            var thisNodesId = StaticGuidUtils.CreateNewId();
            previousNode.AttachNewChildId(thisNodesId);

            var provideInfo = ConversationNode.CreateNew(
                thisNodesId,
                DefaultNodeTypeOptions.ProvideInfo.StringName,
                "",
                intentId,
                "",
                "",
                "",
                accountId,
                DefaultNodeTypeOptions.ProvideInfo.StringName,
                NodeTypeCodeEnum.II,
                false,
                false,
                false,
                false
            );

            return SingleNodeReturnObject.Return(previousNode, provideInfo);
        }
    }
}