using System;
using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models
{
    public class NewConversation
    {
        public string ConversationId { get; set; }
        public List<ConversationNode> ConversationNodes { get; set; }

        private NewConversation(string conversationId, List<ConversationNode> conversationNodes)
        {
            ConversationId = conversationId;
            ConversationNodes = conversationNodes;
        }

        public static NewConversation CreateNew(List<ConversationNode> convoNodes)
        {
            var conversationId = Guid.NewGuid().ToString(); // this is the name of the response PDF
            return new NewConversation(conversationId, convoNodes);
        }
    }
}