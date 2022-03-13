using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Stores
{
    public class EntityStore<TEntity> : IEntityStore<TEntity> where TEntity : class, IEntity
    {
        private readonly IUnitOfWorkContextProvider contextProvider;
        public readonly IAccountIdTransport AccountIdTransport;
        public ICancellationTokenTransport CancellationTokenTransport;
        public readonly IQueryable<TEntity> ReadonlyQueryExecutor;
        public readonly DbSet<TEntity> QueryExecutor;


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

        public EntityStore(IUnitOfWorkContextProvider contextProvider, IAccountIdTransport accountIdTransport, ICancellationTokenTransport cancellationTokenTransport)
        {
            this.contextProvider = contextProvider;
            this.AccountIdTransport = accountIdTransport;
            this.CancellationTokenTransport = cancellationTokenTransport;

            if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
            {
                this.ReadonlyQueryExecutor = ChooseContext(contextProvider).Set<TEntity>().Where(x => ((IHaveAccountId)x).AccountId == AccountIdTransport.AccountId);
            }
            else
            {
                this.ReadonlyQueryExecutor = ChooseContext(contextProvider).Set<TEntity>();
            }

            this.QueryExecutor = ChooseContext(contextProvider).Set<TEntity>();
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
                    throw new DomainException("Account mismatch. Please report this to info.palavy@gmail.com");
                }
            }
        }

        private void AssertAccountIsCorrect(TEntity[] entities)
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

        public async Task<TEntity> Create(TEntity entity)
        {
            AssertAccountIsCorrect(entity);
            var entityEntry = await QueryExecutor.AddAsync(entity, CancellationToken);
            return entityEntry.Entity;
        }

        public async Task CreateMany(TEntity[] entities)
        {
            foreach (var entity in entities)
            {
                AssertAccountIsCorrect(entity);
            }

            await QueryExecutor.AddRangeAsync(entities, CancellationToken);
        }

        public async Task<TEntity[]> Get(Expression<Func<TEntity, bool>> whereFilterPredicate)
        {
            return await ReadonlyQueryExecutor
                .Where(whereFilterPredicate)
                .ToArrayAsync(CancellationToken);
        }

        public async Task<TEntity> Get(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var entity = await ReadonlyQueryExecutor.WhereWorking(id, propertySelectorExpression).SingleOrDefaultAsync(CancellationToken);
            if (entity is null)
            {
                throw new EntityNotFoundException("Entity not found");
            }

            return entity;
        }

        public async Task<TEntity> Get(int id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var entity = await ReadonlyQueryExecutor.WhereWorking(id.ToString(), propertySelectorExpression).SingleAsync(CancellationToken);
            return entity;
        }

        public async Task<TEntity> GetOrNull(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            return await ReadonlyQueryExecutor.WhereWorking(id, propertySelectorExpression).SingleOrDefaultAsync(CancellationToken);
        }

        public async Task<List<TEntity>> GetMany(string[] ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            return await ReadonlyQueryExecutor.WhereWorking(ids, propertySelectorExpression).ToListAsync(CancellationToken);
        }

        public async Task<List<TEntity>> GetMany(List<string> ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            return await ReadonlyQueryExecutor.WhereWorking(ids.ToArray(), propertySelectorExpression).ToListAsync(CancellationToken);
        }


        public Task<List<TEntity>> GetMany(int[] ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var fieldName = propertySelectorExpression.GetMember().Name;
            return ReadonlyQueryExecutor
                .Where(x => ids.Contains((int)x.GetType().GetProperty(fieldName).GetValue(x)))
                .ToListAsync(CancellationToken);
        }

        public async Task<List<TEntity>> GetMany(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var result = await ReadonlyQueryExecutor.WhereWorking(id, propertySelectorExpression).ToListAsync(CancellationToken);
            return result;
        }

        public async Task<List<TEntity>> GetMany(int id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            return await ReadonlyQueryExecutor
                .WhereWorking(id.ToString(), propertySelectorExpression)
                .ToListAsync(CancellationToken);
        }

        public async Task<TEntity[]> GetAll()
        {
            return await ReadonlyQueryExecutor.ToArrayAsync(CancellationToken);
        }

        public async Task<TEntity> GetDeep(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var transientQueryable = ChooseContext(contextProvider)
                .Set<TEntity>()
                .AsNoTracking();

            if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
            {
                transientQueryable = transientQueryable.Where(x => ((IHaveAccountId)x).AccountId == AccountId);
            }

            transientQueryable
                .WhereWorking(id, propertySelectorExpression)
                .Include(((IQueryable<object>)ChooseContext(contextProvider)).GetIncludePaths((IEntityType)typeof(TEntity)));

            return await transientQueryable.SingleAsync(CancellationToken);
        }


        public async Task<TEntity[]> GetAllDeep()
        {
            var transientQueryable = ChooseContext(contextProvider)
                .Set<TEntity>()
                .AsNoTracking();
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
            {
                transientQueryable = transientQueryable.Where(x => ((IHaveAccountId)x).AccountId == AccountId);
            }

            transientQueryable = transientQueryable.Include(((IQueryable<object>)ChooseContext(contextProvider)).GetIncludePaths((IEntityType)typeof(TEntity)));
            return await transientQueryable.ToArrayAsync(CancellationToken);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            await Task.CompletedTask;
            AssertAccountIsCorrect(entity);
            var entityEntry = QueryExecutor.Update(entity);
            return entityEntry.Entity;
        }

        public async Task Delete(TEntity entity)
        {
            await Task.CompletedTask;
            AssertAccountIsCorrect(entity);
            QueryExecutor.Remove(entity);
        }

        public async Task Delete(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var entities = await QueryExecutor.WhereWorking(id, propertySelectorExpression).ToListAsync(CancellationToken);
            QueryExecutor.RemoveRange(entities);
        }

        public async Task Delete(TEntity[] entities)
        {
            await Task.CompletedTask;
            AssertAccountIsCorrect(entities);
            QueryExecutor.RemoveRange(entities);
        }

        public async Task Delete(string[] ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var entities = await GetMany(ids, propertySelectorExpression);
            AssertAccountIsCorrect(entities.ToArray());
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
    }

    public static class BorrowedExtensionMethods
    {
        // I don't really understand this extension method. I'm not sure why this works or why it needs to be written this way.
        // my general gist is that it is a more literal query written with SQL-like syntax via these EF Core functions

        public static IQueryable<TEntity> WhereWorking<TEntity>(
            this IQueryable<TEntity> query,
            string id,
            Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            if (string.IsNullOrEmpty(id) || !(propertySelectorExpression.Body is MemberExpression memberExpression))
            {
                return query;
            }

            // get method info for EF.Functions.Like
            var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
                nameof(DbFunctionsExtensions.Like), new[]
                {
                    typeof(DbFunctions),
                    typeof(string),
                    typeof(string)
                });
            var searchValueConstant = Expression.Constant($"%{id}%");
            var dbFunctionsConstant = Expression.Constant(EF.Functions);
            var propertyInfo = typeof(TEntity).GetProperty(memberExpression.Member.Name);
            var parameterExpression = Expression.Parameter(typeof(TEntity));
            var propertyExpression = Expression.Property(parameterExpression, propertyInfo);


            var callLikeExpression = Expression.Call(likeMethod, dbFunctionsConstant, propertyExpression, searchValueConstant);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(callLikeExpression, parameterExpression);
            return query.Where(lambda);
        }

        public static IQueryable<TEntity> WhereWorking<TEntity>(
            this IQueryable<TEntity> query,
            string[] ids,
            Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            if (ids.Length == 0 || !(propertySelectorExpression.Body is MemberExpression memberExpression))
            {
                return query;
            }

            // get method info for EF.Functions.Like
            var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
                nameof(DbFunctionsExtensions.Like), new[]
                {
                    typeof(DbFunctions),
                    typeof(string),
                    typeof(string)
                });
            var searchValueConstant = Expression.Constant($"%{string.Join(",", ids)}%");
            var dbFunctionsConstant = Expression.Constant(EF.Functions);
            var propertyInfo = typeof(TEntity).GetProperty(memberExpression.Member.Name);
            var parameterExpression = Expression.Parameter(typeof(TEntity));
            var propertyExpression = Expression.Property(parameterExpression, propertyInfo);

            var callLikeExpression = Expression.Call(likeMethod, dbFunctionsConstant, propertyExpression, searchValueConstant);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(callLikeExpression, parameterExpression);
            return query.Where(lambda);
        }
    }
}