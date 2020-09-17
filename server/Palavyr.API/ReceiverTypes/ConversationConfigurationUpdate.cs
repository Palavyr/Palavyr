using System.Collections.Generic;
using Server.Domain;

namespace Palavyr.API.receiverTypes
{
    public class ConversationConfigurationUpdate
    {
        public List<string> IdsToDelete { get; set; }
        public List<ConversationNode> Transactions { get; set; }
    }
}