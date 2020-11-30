using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;
using Server.Domain.Configuration.Constant;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Accounts.Setup.SeedData.DataCreators
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

            var conversationNodes = new List<ConversationNode>()
            {
                DefaultNodeTypeOptions.CreateYesNo().MapNodeTypeOptionToConversationNode(
                    node1Id,
                    "Do you love dogs?",
                    true,
                    new[]{node2Id, node3Id}.Join(Delimiters.NodeChildrenStringDelimiter),
                    DefaultNodeTypeOptions.YesNo.StringName,
                    accountId, 
                    areaIdentifier,
                    null
                ),
                DefaultNodeTypeOptions.CreateTooComplicated().MapNodeTypeOptionToConversationNode(
                    node2Id,
                    "Thats too bad! We should talk.",
                    false,
                    "",
                    DefaultNodeTypeOptions.TooComplicated.StringName,
                    accountId, 
                    areaIdentifier,
                    DefaultNodeTypeOptions.YesNo.No
                ),
                DefaultNodeTypeOptions.CreateYesNo().MapNodeTypeOptionToConversationNode(
                    node3Id,
                    "Do you love Cavvies?",
                    false,
                    new[]{node4Id, node5Id}.Join(Delimiters.NodeChildrenStringDelimiter),
                    DefaultNodeTypeOptions.YesNo.StringName,
                    accountId,
                    areaIdentifier,
                    DefaultNodeTypeOptions.YesNo.Yes
                ),
                
                DefaultNodeTypeOptions.CreateTooComplicated().MapNodeTypeOptionToConversationNode(
                    node4Id,
                    "How can you not love cavvies?? We MUST talk!",
                    false,
                    "",
                    DefaultNodeTypeOptions.TooComplicated.StringName,
                    accountId,
                    areaIdentifier,
                    DefaultNodeTypeOptions.YesNo.No
                ),
                // Dynamic table node doesn't have default creator method
                new ConversationNode()
                {
                    NodeId = node5Id,
                    AreaIdentifier = areaIdentifier,
                    Text = "Which kind of cavvy do you prefer!",
                    IsRoot = false,
                    NodeChildrenString = $"{node6Id}",
                    NodeType = $"SelectOneFlat-{dynamicTableId}",
                    OptionPath = DefaultNodeTypeOptions.YesNo.Yes,
                    ValueOptions = new []{"Ruby", "Black and Tan", "Blenheim"}.Join(Delimiters.PathOptionDelimiter),
                    AccountId = accountId,
                    IsMultiOptionType = true,
                    IsTerminalType = false
                },
                DefaultNodeTypeOptions.CreateEndingSequence().MapNodeTypeOptionToConversationNode(
                    node6Id,
                    "Thank you so much!",
                    false,
                    "",
                    DefaultNodeTypeOptions.EndingSequence.StringName,
                    accountId, 
                    areaIdentifier,
                    DefaultNodeTypeOptions.YesNo.Yes
                )
            };
            return conversationNodes;
        }
    }
}