using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models.Nodes
{
    public class NodeCounter
    {
        public int CountNumTerminal(ConversationNode[] nodeList)
        {
            return nodeList.Count(node => node.IsTerminalType);
        }
    }
}