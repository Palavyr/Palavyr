using System.Collections.Generic;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources.Requests;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public interface IDynamicTable<TEntity> where TEntity : class
    {
        public TEntity CreateTemplate(string accountId, string areaIdentifier, string tableId);
        public List<TEntity> UpdateTable(DynamicTable table);
        public bool EnsureValid();
    }
}