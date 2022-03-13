using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Stores
{
    public interface IEntityStore<TEntity> where TEntity : class, IEntity
    {
        void ResetCancellationToken(CancellationTokenSource tokenSource);
        Task<TEntity> Create(TEntity entity);
        Task CreateMany(TEntity[] entities);
        Task<TEntity> Get(string id, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<TEntity> Get(int id, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<TEntity> GetOrNull(string id, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<List<TEntity>> GetMany(string[] ids, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<List<TEntity>> GetMany(int[] ids, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<List<TEntity>> GetMany(string id, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<List<TEntity>> GetMany(int id, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<TEntity[]> GetAll();
        Task<TEntity[]> GetAllDeep();
        Task Delete(string[] ids, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<TEntity> Update(TEntity entity);
        Task Delete(TEntity entity);
        Task Delete(string id, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task Delete(TEntity[] entities);

        IQueryable<TEntity> Query();

        /// <summary>
        /// DANGEROUS: Only use this if you need to get around accountId filtering.
        /// That should be essentially 'never'
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> RawReadonlyQuery(); // Dangerous - don't use this if you can avoid it. Only need theis when you don't want account filtering.

        CancellationToken CancellationToken { get; }
        string AccountId { get; }
    }
    
}