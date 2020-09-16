using System;
using System.Data;

namespace Server.Domain
{
    public class ConversationUpdate
    {
        public int Id { get; set; }
        public string ConversationId { get; set; }
        public string Prompt { get; set; }
        public string UserResponse { get; set; }
        public string NodeId { get; set; }
        public bool NodeCritical { get; set; }
        public string NodeType { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsCompleted { get; set; }
        public string AccountId { get; set; }
        
        public static ConversationUpdate CreateNew(string conversationId, string prompt, string userResponse, string nodeId, bool nodeCritical, string nodeType, string accountId)
        {
            return new ConversationUpdate()
            {
                ConversationId = conversationId,
                Prompt = prompt,
                UserResponse = userResponse,
                NodeId = nodeId,
                NodeCritical = nodeCritical,
                NodeType = nodeType,
                TimeStamp = DateTime.UtcNow,
                IsCompleted = false,
                AccountId = accountId
            };
        }

        public ConversationUpdate CreateFromPartial(string accountId)
        {
            return new ConversationUpdate()
            {
                ConversationId = ConversationId,
                Prompt = Prompt,
                UserResponse = UserResponse,
                NodeId = NodeId,
                NodeCritical = NodeCritical,
                NodeType = NodeType,
                TimeStamp = DateTime.UtcNow,
                IsCompleted = false,
                AccountId = accountId
            };
        }
        
    }
}