using System;
using System.Collections.Generic;
using Server.Domain;
using Server.Domain.Configuration.constants;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    public static class CreateDefaultConversation
    {
        public static List<ConversationNode> CreateDefault(string accountId, string areaIdentifier,
            string dynamicTableId)
        {
            var node1Id = Guid.NewGuid().ToString(); // Do you love dogs?
            var node2Id = Guid.NewGuid().ToString(); // No / Too Complicated
            var node3Id = Guid.NewGuid().ToString(); // Yes / Do you love cavvies?
            var node4Id = Guid.NewGuid().ToString(); // No / Too Complicated
            var node5Id = Guid.NewGuid().ToString(); // Yes / What kind of Cavvy would you like?
            var node6Id = Guid.NewGuid().ToString(); // SelectOneFlat

            var conversationNodes = new List<ConversationNode>()
            {
                new ConversationNode()
                {
                    NodeId = node1Id,
                    Text = "Do you love dogs?",
                    IsRoot = true,
                    NodeChildrenString = $"{node2Id},{node3Id}", //"node-456,node-789",
                    NodeType = NodeTypes.YesNo,
                    OptionPath = null,
                    ValueOptions = null,
                    AccountId = accountId,
                    AreaIdentifier = areaIdentifier,
                },
                new ConversationNode()
                {
                    NodeId = node2Id,
                    AreaIdentifier = areaIdentifier,
                    Text = "Thats too bad! We should talk.",
                    IsRoot = false,
                    NodeChildrenString = "",
                    NodeType = NodeTypes.TooComplicated,
                    OptionPath = "No",
                    ValueOptions = null,
                    AccountId = accountId
                },
                new ConversationNode()
                {
                    NodeId = node3Id,
                    AreaIdentifier = areaIdentifier,
                    Text = "Do you love Cavvies?",
                    IsRoot = false,
                    NodeChildrenString = $"{node4Id},{node5Id}",
                    NodeType = NodeTypes.YesNo,
                    OptionPath = "Yes",
                    ValueOptions = null,
                    AccountId = accountId
                },
                new ConversationNode()
                {
                    NodeId = node4Id,
                    AreaIdentifier = areaIdentifier,
                    Text = "How can you not love cavvies?? We MUST talk!",
                    IsRoot = false,
                    NodeChildrenString = "",
                    NodeType = NodeTypes.TooComplicated,
                    OptionPath = "No",
                    ValueOptions = null,
                    AccountId = accountId
                },
                new ConversationNode()
                {
                    NodeId = node5Id,
                    AreaIdentifier = areaIdentifier,
                    Text = "Which kind of cavvy do you prefer!",
                    IsRoot = false,
                    NodeChildrenString = $"{node6Id}",
                    NodeType = $"SelectOneFlat-{dynamicTableId}",
                    OptionPath = "Yes",
                    ValueOptions = "Ruby|peg|Black and Tan|peg|Blenheim",
                    AccountId = accountId
                },
                
                new ConversationNode()
                {
                    NodeId = node6Id,
                    AreaIdentifier = areaIdentifier,
                    Text = "Thank you so much!",
                    IsRoot = false,
                    NodeChildrenString = "",
                    NodeType = NodeTypes.EndingSequence,
                    OptionPath = "Yes",
                    ValueOptions = null,
                    AccountId = accountId
                },
            };
            return conversationNodes;
        }
    }
}