﻿using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Test.Common.Constants;

namespace Test.Common.Builders.Conversations.NodeAdders
{
    public static class YesNoExtensionMethod
    {
        public static MultiNodeReturnObject AddYesNo(
            this ConversationNode previousNode,
            string areaId = DefaultConstants.AreaIdentifier,
            string accountId = DefaultConstants.AccountId
        )
        {
            var newId = StaticGuidUtils.CreateNewId();
            previousNode.AttachNewChildId(newId);
            var currentNode = ConversationNode.CreateNew(
                newId,
                DefaultNodeTypeOptions.YesNo.StringName,
                "",
                areaId,
                "",
                "",
                TreeUtils.JoinValueOptionsOnDelimiter(DefaultNodeTypeOptions.YesNo.No, DefaultNodeTypeOptions.YesNo.Yes),
                accountId,
                DefaultNodeTypeOptions.YesNo.StringName,
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