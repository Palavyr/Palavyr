using System.Collections.Generic;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Data.Entities
{
    public interface IPricingStrategyTable<TEntity>
        : IHaveAPrettyNameAndTableType, ICreatePricingStrategyTemplate<TEntity>
        where TEntity : class, IEntity
    {
        public List<TEntity> UpdateTable(PricingStrategyTable<TEntity> table);
        public bool EnsureValid();
    }

    public interface IHaveAPrettyNameAndTableType
    {
        string GetPrettyName();
        string GetTableType();
    }

    public interface ICreatePricingStrategyTemplate<TEntity> where TEntity : class, IEntity
    {
        public TEntity CreateTemplate(string accountId, string intentId, string tableId);
    }
}