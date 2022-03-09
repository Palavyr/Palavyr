using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models
{
    public interface ITreeRootFinder
    {
        ConversationNode GetRootNode(ConversationNode[] nodeList);
    }

    public class TreeRootFinder : ITreeRootFinder
    {
        public ConversationNode GetRootNode(ConversationNode[] nodeList)
        {
            return nodeList.Single(row => row.IsRoot);
        }
    }
}