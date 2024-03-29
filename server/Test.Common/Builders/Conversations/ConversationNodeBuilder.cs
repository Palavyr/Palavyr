﻿using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Test.Common.Constants;

namespace Test.Common.Builders.Conversations
{
    public static partial class BuilderExtensionMethods
    {
        public static ConversationNodeBuilder CreateConversationNodeBuilder(this IUnitTestFixture _)
        {
            return new ConversationNodeBuilder();
        }
    }

    public class ConversationNodeBuilder
    {
        public ConversationNode CreateRootNode(string accountId = DefaultConstants.AccountId, string intentId = DefaultConstants.IntentId)
        {
            return new ConversationNode
            {
                IsRoot = true,
                NodeId = StaticGuidUtils.CreateNewId(),
                Text = "Root Node",
                IntentId = intentId,
                AccountId = accountId,
            };
        }
    }
}