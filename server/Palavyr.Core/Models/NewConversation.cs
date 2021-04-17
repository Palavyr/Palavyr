using System;
using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Models
{
    public class NewConversation
    {
        public string ConversationId { get; set; }
        public List<WidgetNodeResource> ConversationNodes { get; set; }

        private NewConversation(string conversationId, List<WidgetNodeResource> conversationNodes)
        {
            ConversationId = conversationId;
            ConversationNodes = conversationNodes;
        }

        public static NewConversation CreateNew(List<WidgetNodeResource> convoNodes)
        {
            var conversationId = Guid.NewGuid().ToString(); // this is the name of the response PDF
            return new NewConversation(conversationId, convoNodes);
        }
    }
}