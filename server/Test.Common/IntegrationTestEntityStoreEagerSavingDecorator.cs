using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<TEntity> Create(TEntity entity)
        {
            var result = await inner.Create(entity);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
            return result;
        }

        public async Task CreateMany(TEntity[] entities)
        {
            await inner.CreateMany(entities);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }

        public async Task<TEntity> Get(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await inner.Get(id, propertySelectorExpression);
            return result;
        }

        public async Task<TEntity> Get(int id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await inner.Get(id, propertySelectorExpression);
            return result;
        }

        public async Task<TEntity> GetOrNull(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            return await inner.GetOrNull(id, propertySelectorExpression);
        }

        public async Task<List<TEntity>> GetMany(string[] ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await inner.GetMany(ids, propertySelectorExpression);
            return result;
        }

        public async Task<List<TEntity>> GetMany(int[] ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await inner.GetMany(ids, propertySelectorExpression);
            return result;
        }

        public async Task<List<TEntity>> GetMany(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await inner.GetMany(id, propertySelectorExpression);
            return result;
        }

        public async Task<List<TEntity>> GetMany(int id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await inner.GetMany(id, propertySelectorExpression);
            return result;
        }

        public async Task<TEntity[]> GetAll()
        {
            return await inner.GetAll();
        }

        public async Task<TEntity[]> GetAllDeep()
        {
            return await inner.GetAllDeep();
        }

        public async Task Delete(string[] ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            await inner.Delete(ids, propertySelectorExpression);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var result = await inner.Update(entity);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
            return result;
        }

        public async Task Delete(TEntity entity)
        {
            await inner.Delete(entity);
        }

        public async Task Delete(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            await inner.Delete(id, propertySelectorExpression);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }

        public async Task Delete(TEntity[] entities)
        {
            await inner.Delete(entities);
        }

        public IQueryable<TEntity> Query()
        {
            return inner.Query(); // users need to commit manually here;
        }

        public IQueryable<TEntity> RawReadonlyQuery()
        {
            return inner.RawReadonlyQuery();
        }

        public CancellationToken CancellationToken => inner.CancellationToken;
        public string AccountId => inner.AccountId;
    }
}