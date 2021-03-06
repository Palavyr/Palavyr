using System.Collections.Generic;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.Domain.Resources.Requests
{
    public class ConversationNodeDto
    {
        public List<ConversationNode> Transactions { get; set; }
    }
}