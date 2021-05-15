using System;
using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Data.Setup.SeedData.DataCreators
{
    public static class CreateDefaultConversation
    {
        public static List<ConversationNode> CreateDefault(
            string accountId,
            string areaIdentifier,
            string dynamicTableId
        )
        {
            var node1Id = GuidUtils.CreateNewId(); // Do you love dogs?
            var node2Id = GuidUtils.CreateNewId(); // No / Too Complicated
            var node3Id = GuidUtils.CreateNewId(); // Yes / Do you love cavvies?
            var node4Id = GuidUtils.CreateNewId(); // No / Too Complicated
            var node5Id = GuidUtils.CreateNewId(); // Yes / What kind of Cavvy would you like?
            var node6Id = GuidUtils.CreateNewId(); // SelectOneFlat

            return new List<ConversationNode>()
            {
                DefaultNodeTypeOptions.CreateYesNo().MapNodeTypeOptionToConversationNode(
                    node1Id,
                    "Do you love dogs?",
                    true,
                    TreeUtils.CreateNodeChildrenString(node3Id, node2Id),
                    DefaultNodeTypeOptions.YesNo.StringName,
                    accountId,
                    areaIdentifier,
                    null,
                    false
                ),
                DefaultNodeTypeOptions.CreateTooComplicated().MapNodeTypeOptionToConversationNode(
                    node2Id,
                    "Thats too bad! We should talk.",
                    false,
                    "",
                    DefaultNodeTypeOptions.TooComplicated.StringName,
                    accountId,
                    areaIdentifier,
                    DefaultNodeTypeOptions.YesNo.No,
                    false
                ),
                DefaultNodeTypeOptions.CreateYesNo().MapNodeTypeOptionToConversationNode(
                    node3Id,
                    "Do you love Cavvies?",
                    false,
                    TreeUtils.CreateNodeChildrenString(node5Id, node4Id),
                    DefaultNodeTypeOptions.YesNo.StringName,
                    accountId,
                    areaIdentifier,
                    DefaultNodeTypeOptions.YesNo.Yes,
                    false
                ),

                DefaultNodeTypeOptions.CreateTooComplicated().MapNodeTypeOptionToConversationNode(
                    node4Id,
                    "How can you not love cavvies?? We MUST talk!",
                    false,
                    "",
                    DefaultNodeTypeOptions.TooComplicated.StringName,
                    accountId,
                    areaIdentifier,
                    DefaultNodeTypeOptions.YesNo.No,
                    false
                ),
                // Dynamic table node doesn't have default creator method
                new ConversationNode
                {
                    NodeId = node5Id,
                    AreaIdentifier = areaIdentifier,
                    Text = "Which kind of cavvy do you prefer!",
                    IsRoot = false,
                    NodeChildrenString = node6Id,
                    NodeType = $"SelectOneFlat-{dynamicTableId}",
                    OptionPath = DefaultNodeTypeOptions.YesNo.Yes,
                    ValueOptions = string.Join(Delimiters.PathOptionDelimiter, new[] {"Ruby", "Black and Tan", "Blenheim"}),
                    AccountId = accountId,
                    IsMultiOptionType = true,
                    IsTerminalType = false,
                    IsDynamicTableNode = true,
                    ShouldRenderChildren = true,
                    NodeComponentType = DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue
                },
                DefaultNodeTypeOptions.CreateSendResponse().MapNodeTypeOptionToConversationNode(
                    node6Id,
                    "Thank you so much!",
                    false,
                    "",
                    DefaultNodeTypeOptions.SendResponse.StringName,
                    accountId,
                    areaIdentifier,
                    "Continue",
                    false
                )
            };
        }
    }
}