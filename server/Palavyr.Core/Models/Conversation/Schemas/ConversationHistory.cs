﻿#nullable disable

using System;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Conversation.Schemas
{
    public class ConversationHistory : Entity, IHaveAccountId
    {
        public string ConversationId { get; set; }
        public string Prompt { get; set; }
        public string UserResponse { get; set; }
        public string NodeId { get; set; }
        public bool NodeCritical { get; set; }
        public string NodeType { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AccountId { get; set; }

        public static ConversationHistory CreateNew(
            string conversationId,
            string prompt,
            string userResponse,
            string nodeId,
            bool nodeCritical,
            string nodeType,
            string accountId)
        {
            return new ConversationHistory
            {
                ConversationId = conversationId,
                Prompt = prompt,
                UserResponse = userResponse,
                NodeId = nodeId,
                NodeCritical = nodeCritical,
                NodeType = nodeType,
                TimeStamp = DateTime.UtcNow,
                AccountId = accountId
            };
        }
    }
}