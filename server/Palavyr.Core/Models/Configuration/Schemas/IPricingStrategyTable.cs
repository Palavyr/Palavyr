using System.Collections.Generic;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public interface IPricingStrategyTable<TEntity> where TEntity : class
    {
        public TEntity CreateTemplate(string accountId, string areaIdentifier, string tableId);
        public List<TEntity> UpdateTable(PricingStrategyTable<TEntity> table);
        public bool EnsureValid();
    }
}