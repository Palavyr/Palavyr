using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Stores
{
    public interface IEntityStore<TEntity> where TEntity : class, IEntity
    {
        void ResetCancellationToken(CancellationTokenSource tokenSource);
        Task<TEntity> Get(string id, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<TEntity> Get(int id);
        
        Task<TEntity?> GetOrNull(string id, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<TEntity?> GetOrNull(int id);

        Task<List<TEntity>> GetMany(IEnumerable<string> ids, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<List<TEntity>> GetMany(string id, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task<List<TEntity>> GetMany(IEnumerable<int> ids);
        Task<TEntity[]> GetAll();
        Task<TEntity[]> GetPaginated(int skip, int take);
        Task<TEntity> Create(TEntity entity);
        Task CreateMany(IEnumerable<TEntity> entities);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> CreateOrUpdate(TEntity entity);
        Task<List<TEntity>> CreateOrUpdateMany(IEnumerable<TEntity> entities);
        Task Delete(IEnumerable<string> ids, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task DeleteMany(IEnumerable<int> ids);
        Task Delete(int id);
        Task Delete(TEntity entity);
        Task Delete(string id, Expression<Func<TEntity, string>> propertySelectorExpression);
        Task Delete(IEnumerable<TEntity> entities);

        IQueryable<TEntity> Query();


        DbSet<TEntity> DangerousRawQuery();

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