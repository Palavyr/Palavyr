using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models.Resources.Requests
{
    public class ConversationNodeDto
    {
        public List<ConversationNode> Transactions { get; set; }
    }
}