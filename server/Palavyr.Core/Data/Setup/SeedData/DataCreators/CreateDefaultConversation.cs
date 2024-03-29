﻿using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Data.Setup.SeedData.DataCreators
{
    public static class CreateDefaultConversation
    {
        public static List<ConversationNode> CreateDefault(
            string accountId,
            string intentId,
            string pricingStrategyTableId
        )
        {
            var node1Id = StaticGuidUtils.CreateNewId(); // Do you love dogs?
            var node2Id = StaticGuidUtils.CreateNewId(); // No / Too Complicated
            var node3Id = StaticGuidUtils.CreateNewId(); // Yes / Do you love dogs?
            var node4Id = StaticGuidUtils.CreateNewId(); // No / Too Complicated
            var node5Id = StaticGuidUtils.CreateNewId(); // Yes / What kind of dog would you like?
            var node6Id = StaticGuidUtils.CreateNewId(); // SelectOneFlat

            return new List<ConversationNode>()
            {
                DefaultNodeTypeOptions.CreateYesNo().MapNodeTypeOptionToConversationNode(
                    node1Id,
                    "Do you love animals?",
                    true,
                    TreeUtils.CreateNodeChildrenString(node3Id, node2Id),
                    DefaultNodeTypeOptions.YesNo.StringName,
                    accountId,
                    intentId,
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
                    intentId,
                    DefaultNodeTypeOptions.YesNo.No,
                    false
                ),
                DefaultNodeTypeOptions.CreateYesNo().MapNodeTypeOptionToConversationNode(
                    node3Id,
                    "Do you love dogs?",
                    false,
                    TreeUtils.CreateNodeChildrenString(node5Id, node4Id),
                    DefaultNodeTypeOptions.YesNo.StringName,
                    accountId,
                    intentId,
                    DefaultNodeTypeOptions.YesNo.Yes,
                    false
                ),

                DefaultNodeTypeOptions.CreateTooComplicated().MapNodeTypeOptionToConversationNode(
                    node4Id,
                    "How can you not love dogs!? We MUST talk!",
                    false,
                    "",
                    DefaultNodeTypeOptions.TooComplicated.StringName,
                    accountId,
                    intentId,
                    DefaultNodeTypeOptions.YesNo.No,
                    false
                ),
                // Pricing strategy table node doesn't have default creator method
                new ConversationNode
                {
                    NodeId = node5Id,
                    IntentId = intentId,
                    Text = "Which kind of dog do you prefer!",
                    IsRoot = false,
                    NodeChildrenString = node6Id,
                    NodeType = $"SelectOneFlat-{pricingStrategyTableId}",
                    OptionPath = DefaultNodeTypeOptions.YesNo.Yes,
                    ValueOptions = string.Join(Delimiters.PathOptionDelimiter, new[] {"Ruby", "Black and Tan", "Blenheim"}),
                    AccountId = accountId,
                    IsMultiOptionType = true,
                    IsTerminalType = false,
                    IsPricingStrategyTableNode = true,
                    ShouldRenderChildren = true,
                    NodeComponentType = DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    NodeTypeCodeEnum = NodeTypeCodeEnum.III,
                    PricingStrategyType = $"SelectOneFlat-{pricingStrategyTableId}"
                },
                DefaultNodeTypeOptions.CreateSendResponse().MapNodeTypeOptionToConversationNode(
                    node6Id,
                    "Thank you so much!",
                    false,
                    "",
                    DefaultNodeTypeOptions.SendResponse.StringName,
                    accountId,
                    intentId,
                    "Continue",
                    false
                )
            };
        }
    }
}