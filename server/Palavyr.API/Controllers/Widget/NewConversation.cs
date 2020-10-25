using System;
using System.Collections.Generic;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    public class NewConversation
    {
        public string ConversationId { get; set; }
        public WidgetPreference WidgetPreference { get; set; }
        public List<ConversationNode> ConversationNodes { get; set; }

        NewConversation(string conversationId, WidgetPreference widgetPreference, List<ConversationNode> conversationNodes)
        {
            ConversationId = conversationId;
            WidgetPreference = widgetPreference;
            ConversationNodes = conversationNodes;
        }

        public static NewConversation CreateNew(WidgetPreference widgetPreference, List<ConversationNode> convoNodes)
        {
            var conversationId = Guid.NewGuid().ToString(); // this is the name of the response PDF
            return new NewConversation(conversationId, widgetPreference, convoNodes);
        }
    }
}