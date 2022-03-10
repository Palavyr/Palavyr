using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Repositories.StoreExtensionMethods
{
    public static class ConversationNodeExtensionMethods
    {
        public static async Task<List<ConversationNode>> UpdateConversation(this IEntityStore<Area> intentStore, string intentId, List<ConversationNode> convoUpdate)
        {
            var intent = await intentStore
                .Query()
                .Where(row => row.AreaIdentifier == intentId)
                .Include(p => p.ConversationNodes)
                .SingleOrDefaultAsync();

            intent.ConversationNodes.Clear();
            intent.ConversationNodes.AddRange(convoUpdate);
            return convoUpdate;
        }

        public static async Task<ConversationNode?> UpdateConversationNodeText(this IEntityStore<ConversationNode> convoNodeStore, string areaId, string nodeId, string nodeTextUpdate)
        {
            var node = await convoNodeStore.Get(nodeId, s => s.NodeId);
            if (node != null)
            {
                node.Text = nodeTextUpdate;
            }

            return node;
        }
    }
}