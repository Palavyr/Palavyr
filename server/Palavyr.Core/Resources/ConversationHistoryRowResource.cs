using System;

namespace Palavyr.Core.Resources
{
    public class ConversationHistoryRowResource : NullableEntityResource
    {
        public string ConversationId { get; set; }
        public string Prompt { get; set; }
        public string UserResponse { get; set; }
        public string NodeId { get; set; }
        public bool NodeCritical { get; set; }
        public string NodeType { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}