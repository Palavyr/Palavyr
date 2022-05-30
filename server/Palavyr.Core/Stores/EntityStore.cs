#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Stores
{
    public class EntityStore<TEntity> : IEntityStore<TEntity> where TEntity : class, IEntity
    {
        public readonly IAccountIdTransport AccountIdTransport;
        public ICancellationTokenTransport CancellationTokenTransport;
        public readonly DbSet<TEntity> QueryExecutor;

        public static readonly string AccountMismatchErrorString = "Account mismatch. Please report this to info.palavy@gmail.com";

        public CancellationToken CancellationToken => CancellationTokenTransport.CancellationToken;
        public string AccountId => AccountIdTransport.AccountId;

        private Type[] accountContextTypes = new[] // separated out because of a poor decision I made early on. All new tables will go into the configuration context
        {
            typeof(Account),
            typeof(EmailVerification),
            typeof(Session),
            typeof(StripeWebhookReceivedRecord),
            typeof(Subscription)
        };

        private Type[] convoTypes = new[]
        {
            typeof(ConversationHistory),
            typeof(ConversationRecord)
        };

        private readonly DbContext currentContext;

        public EntityStore(IUnitOfWorkContextProvider contextProvider, IAccountIdTransport accountIdTransport, ICancellationTokenTransport cancellationTokenTransport)
        {
            this.AccountIdTransport = accountIdTransport;
            this.CancellationTokenTransport = cancellationTokenTransport;
            this.currentContext = ChooseContext(contextProvider);
            this.QueryExecutor = ChooseContext(contextProvider).Set<TEntity>();
        }

        private IQueryable<TEntity> RestrictToCurrentAccount(DbSet<TEntity> queryExecutor)
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
            {
                return queryExecutor.Where(x => ((IHaveAccountId)x).AccountId == AccountIdTransport.AccountId);
            }

            return queryExecutor;
        }

        private IEnumerable<TEntity> RestrictToCurrentAccount(LocalView<TEntity> localEntities)
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
            {
                return localEntities.Where(x => ((IHaveAccountId)x).AccountId == AccountIdTransport.AccountId);
            }

            return localEntities;
        }


        private DbContext ChooseContext(IUnitOfWorkContextProvider contextProvider)
        {
            if (accountContextTypes.Contains(typeof(TEntity)))
            {
                return contextProvider.AccountsContext();
            }
            else if (convoTypes.Contains(typeof(TEntity)))
            {
                return contextProvider.ConvoContext();
            }
            else
            {
                return contextProvider.ConfigurationContext();
            }
        }

        private void AssertAccountIsCorrect(TEntity entity)
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
            {
                if (((IHaveAccountId)entity).AccountId != AccountId)
                {
                    throw new AccountMisMatchException(AccountMismatchErrorString);
                }
            }
        }

        private void AssertAccountIsCorrect(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                AssertAccountIsCorrect(entity);
            }
        }

        public void ResetCancellationToken(CancellationTokenSource tokenSource)
        {
            this.CancellationTokenTransport = new CancellationTokenTransport(tokenSource.Token);
        }

        public async Task<TEntity[]> GetPaginated(int skip, int take)
        {
            var page = await QueryExecutor.Skip(skip).Take(take).ToArrayAsync(CancellationToken);
            return page;
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            AssertAccountIsCorrect(entity);
            var entityEntry = await QueryExecutor.AddAsync(entity, CancellationToken);
            return entityEntry.Entity;
        }


        public async Task CreateMany(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                AssertAccountIsCorrect(entity);
            }

            await QueryExecutor.AddRangeAsync(entities, CancellationToken);
        }

        public async Task<TEntity[]> Get(Expression<Func<TEntity, bool>> whereFilterPredicate)
        {
            return await RestrictToCurrentAccount(QueryExecutor)
                .Where(whereFilterPredicate)
                .ToArrayAsync(CancellationToken);
        }

        public async Task<TEntity> Get(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var entity = await RestrictToCurrentAccount(QueryExecutor)
                .CustomWhere(id, propertySelectorExpression)
                .SingleOrDefaultAsync(CancellationToken);
            if (entity is null)
            {
                entity = RestrictToCurrentAccount(QueryExecutor.Local).SingleOrDefault();
                if (entity is null)
                {
                    throw new EntityNotFoundException("Entity not found");
                }

                return entity;
            }

            return entity;
        }

        public async Task<TEntity> Get(int id)
        {
            var entity = await RestrictToCurrentAccount(QueryExecutor).Where(x => x.Id == id).SingleOrDefaultAsync(CancellationToken);
            if (entity is null)
            {
                throw new DomainException("Entity not found.");
            }

            return entity;
        }

        public async Task<TEntity?> GetOrNull(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            return await RestrictToCurrentAccount(QueryExecutor).CustomWhere(id, propertySelectorExpression).SingleOrDefaultAsync(CancellationToken);
        }

        public async Task<TEntity> GetOrNull(int id)
        {
            return await RestrictToCurrentAccount(QueryExecutor).Where(x => x.Id == id).SingleOrDefaultAsync(CancellationToken);
        }

        public async Task<List<TEntity>> GetMany(IEnumerable<string> ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            // A hack until I figure out how to property customize CustomWhere with multiple Ids... shouldn't be that difficult...
            var result = new List<TEntity>();
            foreach (var id in ids)
            {
                var entities = await RestrictToCurrentAccount(QueryExecutor)
                    .CustomWhere(id, propertySelectorExpression)
                    .ToListAsync(CancellationToken);
                result.AddRange(entities);
            }

            return result;
        }

        public async Task<List<TEntity>> GetMany(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await RestrictToCurrentAccount(QueryExecutor)
                .CustomWhere(id, propertySelectorExpression)
                .ToListAsync(CancellationToken);
            return result;
        }

        public async Task<List<TEntity>> GetMany(IEnumerable<int> ids)
        {
            var idList = ids.ToList();
            return await RestrictToCurrentAccount(QueryExecutor)
                .Where(x => idList.Contains((int)x.Id))
                .ToListAsync(CancellationToken);
        }

        public async Task<TEntity[]> GetAll()
        {
            return await RestrictToCurrentAccount(QueryExecutor).ToArrayAsync(CancellationToken);
        }


        public async Task<TEntity> Update(TEntity entity)
        {
            if (entity.Id is null)
            {
                throw new InvalidOperationException("Cannot update entities that are not referenced by their primary Id key");
            }

            await Task.CompletedTask;
            AssertAccountIsCorrect(entity);
            var entityEntry = QueryExecutor.Update(entity);
            return entityEntry.Entity;
        }

        public async Task<TEntity> CreateOrUpdate(TEntity entity)
        {
            await Task.CompletedTask;
            AssertAccountIsCorrect(entity);
            if (entity.Id is null)
            {
                var entityEntry = await QueryExecutor.AddAsync(entity, CancellationToken);
                return entityEntry.Entity;
            }
            else
            {
                var entityEntry = QueryExecutor.Update(entity);
                return entityEntry.Entity;
            }
        }

        public async Task<List<TEntity>> CreateOrUpdateMany(IEnumerable<TEntity> entities)
        {
            await Task.CompletedTask;
            foreach (var entity in entities)
            {
                AssertAccountIsCorrect(entity);
            }

            var toReturn = new List<TEntity>();
            foreach (var entity in entities)
            {
                
                if (entity.Id is null)
                {
                    var entityEntry = await QueryExecutor.AddAsync(entity, CancellationToken);
                    toReturn.Add(entityEntry.Entity);
                }
                else
                {
                    var entityEntry = QueryExecutor.Update(entity);
                    toReturn.Add(entityEntry.Entity);
                }
            }

            return toReturn;
        }

        public async Task DeleteMany(IEnumerable<int> ids)
        {
            var entities = await GetMany(ids);
            await Delete(entities);
        }

        public async Task Delete(int id)
        {
            var entity = await Get(id);
            await Delete(entity);
        }

        public async Task Delete(TEntity entity)
        {
            AssertAccountIsCorrect(entity);
            await Delete(new[] { entity });
        }

        public async Task Delete(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var entities = await QueryExecutor.CustomWhere(id, propertySelectorExpression).ToListAsync(CancellationToken);
            await Delete(entities);
        }

        public async Task Delete(IEnumerable<string> ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var entities = await GetMany(ids, propertySelectorExpression);
            await Delete(entities);
        }

        public async Task Delete(IEnumerable<TEntity> entities)
        {
            await Task.CompletedTask;
            AssertAccountIsCorrect(entities);
            QueryExecutor.RemoveRange(entities);
        }


        public IQueryable<TEntity> Query()
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
            {
                return QueryExecutor.Where(x => ((IHaveAccountId)x).AccountId == AccountId);
            }

            throw new DomainException("Raw Queries on entities that do not implement IHaveAccountId are not allowed");
        }

        public DbSet<TEntity> DangerousRawQuery()
        {
            return QueryExecutor;
        }

        public IQueryable<TEntity> RawReadonlyQuery()
        {
            return QueryExecutor.AsNoTracking();
        }


        // public async Task<TEntity> GetDeep(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        // {
        //     var transientQueryable = ChooseContext(contextProvider)
        //         .Set<TEntity>()
        //         .AsNoTracking();
        //
        //     if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
        //     {
        //         transientQueryable = transientQueryable.Where(x => ((IHaveAccountId)x).AccountId == AccountId);
        //     }
        //
        //     transientQueryable
        //         .CustomWhere(id, propertySelectorExpression)
        //         .Include(((IQueryable<object>)ChooseContext(contextProvider)).GetIncludePaths((IEntityType)typeof(TEntity)));
        //
        //     return await transientQueryable.SingleAsync(CancellationToken);
        // }


        // public async Task<TEntity[]> GetAllDeep()
        // {
        //     var transientQueryable = ChooseContext(contextProvider)
        //         .Set<TEntity>()
        //         .AsNoTracking();
        //     if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
        //     {
        //         transientQueryable = transientQueryable.Where(x => ((IHaveAccountId)x).AccountId == AccountId);
        //     }
        //
        //     var context = (IQueryable<object>)transientQueryable;
        //
        //     var pathTypes = (IEntityType)typeof(TEntity);
        //     transientQueryable = transientQueryable.Include(context.GetIncludePaths(pathTypes));
        //     return await transientQueryable.ToArrayAsync(CancellationToken);
        // }
    }
}