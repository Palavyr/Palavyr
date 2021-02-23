using System.Collections.Generic;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.Domain.Resources.Requests
{
    public class ConversationConfigurationUpdate
    {
        public List<string> IdsToDelete { get; set; }
        public List<ConversationNode> Transactions { get; set; }
    }
}