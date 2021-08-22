using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Models.Conversation.Schemas
{
    // TODO: Rename to ConversationHistory
    public class ConversationHistory
    {
        [Key]
        public int? Id { get; set; }

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

        public ConversationHistory CreateFromPartial(string accountId)
        {
            var timeStamp = DateTime.UtcNow;
            
            return new ConversationHistory
            {
                ConversationId = ConversationId,
                Prompt = Prompt,
                UserResponse = UserResponse,
                NodeId = NodeId,
                NodeCritical = NodeCritical,
                NodeType = NodeType,
                TimeStamp = timeStamp,
                AccountId = accountId
            };
        }
    }
}