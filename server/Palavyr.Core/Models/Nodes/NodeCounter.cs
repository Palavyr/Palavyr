using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models.Nodes
{
    public interface INodeCounter
    {
        int CountNumTerminal(ConversationNode[] nodeList);
    }

    public class NodeCounter : INodeCounter
    {
        public int CountNumTerminal(ConversationNode[] nodeList)
        {
            return nodeList.Count(node => node.IsTerminalType);
        }
    }
}