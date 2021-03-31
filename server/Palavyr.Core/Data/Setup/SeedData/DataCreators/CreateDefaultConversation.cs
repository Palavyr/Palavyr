using System;
using System.Collections.Generic;
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
            var node1Id = Guid.NewGuid().ToString(); // Do you love dogs?
            var node2Id = Guid.NewGuid().ToString(); // No / Too Complicated
            var node3Id = Guid.NewGuid().ToString(); // Yes / Do you love cavvies?
            var node4Id = Guid.NewGuid().ToString(); // No / Too Complicated
            var node5Id = Guid.NewGuid().ToString(); // Yes / What kind of Cavvy would you like?
            var node6Id = Guid.NewGuid().ToString(); // SelectOneFlat

            return new List<ConversationNode>()
            {
                DefaultNodeTypeOptions.CreateYesNo().MapNodeTypeOptionToConversationNode(
                    node1Id,
                    "Do you love dogs?",
                    true,
                    TreeUtils.CreateNodeChildrenString(node2Id, node3Id),
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
                    TreeUtils.CreateNodeChildrenString(node4Id, node5Id),
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
                new ConversationNode()
                {
                    NodeId = node5Id,
                    AreaIdentifier = areaIdentifier,
                    Text = "Which kind of cavvy do you prefer!",
                    IsRoot = false,
                    NodeChildrenString = node6Id,
                    NodeType = $"SelectOneFlat-{dynamicTableId}",
                    OptionPath = DefaultNodeTypeOptions.YesNo.Yes,
                    ValueOptions = string.Join(Delimiters.PathOptionDelimiter, new []{"Ruby", "Black and Tan", "Blenheim"}),
                    AccountId = accountId,
                    IsMultiOptionType = true,
                    IsTerminalType = false,
                    IsDynamicTableNode = true,
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
                    DefaultNodeTypeOptions.YesNo.Yes,
                    false
                )
            };
        }
    }
}