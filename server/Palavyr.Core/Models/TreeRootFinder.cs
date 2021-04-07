using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models
{
    public class TreeRootFinder
    {
        public ConversationNode GetRootNode(ConversationNode[] nodeList)
        {
            return nodeList.Single(row => row.IsRoot);
        }

    }
}