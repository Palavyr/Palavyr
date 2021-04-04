using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models
{
    public class NodeCounter
    {
        public int CountNumTerminal(ConversationNode[] nodeList)
        {
            return nodeList.Count(node => node.IsTerminalType);
        }
    }
}