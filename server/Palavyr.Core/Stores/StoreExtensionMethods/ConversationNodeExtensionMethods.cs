﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Stores.StoreExtensionMethods
{
    public static class ConversationNodeExtensionMethods
    {
        public static async Task<List<ConversationNode>> UpdateConversation(this IEntityStore<Intent> intentStore, string intentId, List<ConversationNode> convoUpdate)
        {
            var intent = await intentStore
                .Query()
                .Where(row => row.IntentId == intentId)
                .Include(p => p.ConversationNodes)
                .SingleOrDefaultAsync();

            intent.ConversationNodes.Clear();
            intent.ConversationNodes.AddRange(convoUpdate);
            return convoUpdate;
        }

        public static async Task<ConversationNode?> UpdateConversationNodeText(this IEntityStore<ConversationNode> convoNodeStore, string intentId, string nodeId, string nodeTextUpdate)
        {
            var node = await convoNodeStore.GetOrNull(nodeId, s => s.NodeId);
            if (node != null)
            {
                node.Text = nodeTextUpdate;
            }

            return node;
        }
    }
}