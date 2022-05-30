using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Stores;

namespace Test.Common
{
    public class IntegrationTestEntityStoreEagerSavingDecorator<TEntity> : IEntityStore<TEntity> where TEntity : class, IEntity
    {
        private readonly IEntityStore<TEntity> inner;
        private readonly IUnitOfWorkContextProvider unitOfWorkContextProvider;

        public IntegrationTestEntityStoreEagerSavingDecorator(IEntityStore<TEntity> inner, IUnitOfWorkContextProvider unitOfWorkContextProvider)
        {
            inner.ResetCancellationToken(new CancellationTokenSource(TimeSpan.FromSeconds(60)));
            this.inner = inner;
            this.unitOfWorkContextProvider = unitOfWorkContextProvider;
        }


        public void ResetCancellationToken(CancellationTokenSource tokenSource)
        {
            inner.ResetCancellationToken(tokenSource);
        }

        public async Task<TEntity[]> GetPaginated(int skip, int take)
        {
            return await inner.GetPaginated(skip, take);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            var result = await inner.Create(entity);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
            return result;
        }

        public async Task CreateMany(IEnumerable<TEntity> entities)
        {
            await inner.CreateMany(entities);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }

        public async Task<TEntity> Get(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await inner.Get(id, propertySelectorExpression);
            return result;
        }

        public async Task<TEntity> Get(int id)
        {
            return await inner.Get(id);
        }

        public async Task<TEntity> GetOrNull(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            return await inner.GetOrNull(id, propertySelectorExpression);
        }

        public async Task<TEntity> GetOrNull(int id)
        {
            return await inner.GetOrNull(id);
        }

        public async Task<List<TEntity>> GetMany(IEnumerable<string> ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await inner.GetMany(ids, propertySelectorExpression);
            return result;
        }

        public async Task<List<TEntity>> GetMany(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await inner.GetMany(id, propertySelectorExpression);
            return result;
        }

        public async Task<List<TEntity>> GetMany(IEnumerable<int> ids)
        {
            return await inner.GetMany(ids);
        }

        public async Task<TEntity[]> GetAll()
        {
            return await inner.GetAll();
        }

        public async Task<List<TEntity>> CreateOrUpdateMany(IEnumerable<TEntity> entities)
        {
            var result = await inner.CreateOrUpdateMany(entities);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
            return result;
        }

        public async Task Delete(IEnumerable<string> ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            await inner.Delete(ids, propertySelectorExpression);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }

        public async Task DeleteMany(IEnumerable<int> ids)
        {
            await inner.DeleteMany(ids);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var result = await inner.Update(entity);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
            return result;
        }

        public Task<TEntity> CreateOrUpdate(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(TEntity entity)
        {
            await inner.Delete(entity);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }

        public async Task Delete(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            await inner.Delete(id, propertySelectorExpression);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }

        public async Task Delete(IEnumerable<TEntity> entities)
        {
            await inner.Delete(entities);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }

        public IQueryable<TEntity> Query()
        {
            return inner.Query(); // users need to commit manually here;
        }

        public DbSet<TEntity> DangerousRawQuery()
        {
            return inner.DangerousRawQuery();
        }

        public IQueryable<TEntity> RawReadonlyQuery()
        {
            return inner.RawReadonlyQuery();
        }

        public CancellationToken CancellationToken => inner.CancellationToken;
        public string AccountId => inner.AccountId;
    }
}