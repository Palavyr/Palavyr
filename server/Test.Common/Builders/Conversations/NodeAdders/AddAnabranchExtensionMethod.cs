using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Configuration.Constant;
using Test.Common.Constants;

namespace Test.Common.Builders.Conversations.NodeAdders
{
    public static class AddAnabranchExtensionMethod
    {
        public static MultiNodeReturnObject AddAnabranch(this ConversationNode previousNode, string[] options, string areaId = DefaultConstants.AreaIdentifier, string accountId = DefaultConstants.AccountId)
        {
            var newId = StaticGuidUtils.CreateNewId();
            previousNode.AttachNewChildId(newId);

            var anabranch = ConversationNode.CreateNew(
                StaticGuidUtils.CreateNewId(),
                DefaultNodeTypeOptions.Anabranch.StringName,
                "",
                areaId,
                "",
                "",
                "",
                accountId,
                DefaultNodeTypeOptions.Anabranch.StringName,
                NodeTypeCode.VI,
                false,
                false,
                true,
                false,
                isMultiOptionEditable: false
            );

            return null;

        }
    }
}