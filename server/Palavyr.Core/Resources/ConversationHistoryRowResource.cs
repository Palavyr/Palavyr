namespace Palavyr.Core.Resources
{
    public class ConversationHistoryRowResource
    {
        public string ConversationId { get; set; }
        public string Prompt { get; set; }
        public string UserResponse { get; set; }
        public string NodeId { get; set; }
        public bool NodeCritical { get; set; }
        public string NodeType { get; set; }
        public string TimeStamp { get; set; }
        public string AccountId { get; set; }
    }
}