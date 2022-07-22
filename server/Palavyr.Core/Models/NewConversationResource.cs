using System;
using System.Collections.Generic;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Models
{
    public class NewConversationResource
    {
        public string ConversationId { get; set; }
        public List<WidgetNodeResource> ConversationNodes { get; set; }

        public NewConversationResource()
        {
        }

        private NewConversationResource(string conversationId, List<WidgetNodeResource> conversationNodes)
        {
            ConversationId = conversationId;
            ConversationNodes = conversationNodes;
        }

        public static NewConversationResource CreateNew(List<WidgetNodeResource> convoNodes)
        {
            var conversationId = Guid.NewGuid().ToString(); // this is the id of the response PDF
            return new NewConversationResource(conversationId, convoNodes);
        }
    }
}