﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Stores.StoreExtensionMethods
{
    public static class IntentStoreExtensionMethods
    {
        public static async Task<List<Intent>> GetAllIntentsComplete(this IEntityStore<Intent> intentStore)
        {
            var allIntentsComplete = await intentStore
                .Query()
                .Include(row => row.AttachmentRecords)
                .Include(row => row.ConversationNodes)
                .Include(row => row.PricingStrategyTableMetas)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee)
                .Where(row => row.AccountId == intentStore.AccountId)
                .ToListAsync(intentStore.CancellationToken);
            return allIntentsComplete;
        }
        
        
        public static async Task<Intent> GetIntentComplete(this IEntityStore<Intent> intentStore, string intentId)
        {
            var intentComplete = await intentStore
                .Query()
                .Include(row => row.AttachmentRecords)
                .Include(row => row.ConversationNodes)
                .Include(row => row.PricingStrategyTableMetas)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee)
                .SingleAsync(row => row.IntentId == intentId, intentStore.CancellationToken);
            return intentComplete;
        }

        public static async Task<Intent> GetIntentOnly(this IEntityStore<Intent> intentStore, string intentId)
        {
            var intentComplete = await intentStore.Get(intentId, s => s.IntentId);
            return intentComplete;
        }

        public static async Task<List<Intent>> GetActiveIntentsWithConvoAndPricingStrategyAndStaticTables(this IEntityStore<Intent> intentStore)
        {
            return await intentStore
                .Query()
                .Where(row => row.IsEnabled)
                .Include(row => row.ConversationNodes)
                .Include(row => row.PricingStrategyTableMetas)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(row => row.StaticTableRows)
                .ToListAsync(intentStore.CancellationToken);
        }
    }
}