using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Repositories.StoreExtensionMethods
{
    public static class StaticTableStoreExtensionMethods
    {
        public static async Task<List<StaticTablesMeta>> GetStaticTablesComplete(this IEntityStore<StaticTablesMeta> staticTablesStore, string intentId)
        {
            return await staticTablesStore
                .Query()
                .Where(meta => meta.AreaIdentifier == intentId)
                .Include(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee)
                .ToListAsync(staticTablesStore.CancellationToken);
        }
    }
}