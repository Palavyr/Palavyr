using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Test.Common.Constants;

namespace Test.Common.Builders.Conversations.NodeAdders
{
    public static class YesNoExtensionMethod
    {
        public static MultiNodeReturnObject AddYesNo(
            this ConversationNode previousNode,
            string intentId = DefaultConstants.IntentId,
            string accountId = DefaultConstants.AccountId
        )
        {
            var newId = StaticGuidUtils.CreateNewId();
            previousNode.AttachNewChildId(newId);
            var currentNode = ConversationNode.CreateNew(
                newId,
                DefaultNodeTypeOptions.YesNo.StringName,
                "",
                intentId,
                "",
                "",
                TreeUtils.JoinValueOptionsOnDelimiter(DefaultNodeTypeOptions.YesNo.No, DefaultNodeTypeOptions.YesNo.Yes),
                accountId,
                DefaultNodeTypeOptions.YesNo.StringName,
                NodeTypeCode.V,
                false,
                false,
                true,
                false
            );

            var yesNode = new ConversationNode();
            yesNode.SetOptionPath(DefaultNodeTypeOptions.YesNo.Yes);

            var noNode = new ConversationNode();
            noNode.SetOptionPath(DefaultNodeTypeOptions.YesNo.No);

            return MultiNodeReturnObject.Return(currentNode, yesNode, noNode);
        }
    }
}