using System.Collections.Generic;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.Domain.Configuration.Schemas
{
    public interface IDynamicTable<TEntity> where TEntity : class
    {
        public TEntity CreateTemplate(string accountId, string areaIdentifier, string tableId);
        public List<TEntity> UpdateTable(DynamicTable table);
    }
}