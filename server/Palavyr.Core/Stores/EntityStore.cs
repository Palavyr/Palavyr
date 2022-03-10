using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Internal;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Stores
{
    public class EntityStore<TEntity> : IEntityStore<TEntity> where TEntity : class, IEntity, IHaveAccountId
    {
        private readonly IUnitOfWorkContextProvider contextProvider;
        public readonly IAccountIdTransport AccountIdTransport;
        public readonly ICancellationTokenTransport CancellationTokenTransport;
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
            this.ReadonlyQueryExecutor = ChooseContext(contextProvider).Set<TEntity>().AsNoTracking().Where(x => x.AccountId == accountIdTransport.AccountId);
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
            if (entity.AccountId != AccountIdTransport.AccountId)
            {
                throw new DomainException("Account mismatch. Please report this to info.palavy@gmail.com");
            }
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            AssertAccountIsCorrect(entity);
            var entityEntry = await QueryExecutor.AddAsync(entity, CancellationToken);
            return entityEntry.Entity;
        }

        public async Task CreateMany(TEntity[] entities)
        {
            await Task.CompletedTask;
            await QueryExecutor.AddRangeAsync(entities, CancellationToken);
        }

        public async Task<TEntity[]> Get(Expression<Func<TEntity, bool>> whereFilterPredicate)
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
            {
                return await ReadonlyQueryExecutor
                    .Where(x => x.AccountId == AccountIdTransport.AccountId)
                    .Where(whereFilterPredicate)
                    .ToArrayAsync(CancellationToken);
            }

            return await ReadonlyQueryExecutor.Where(whereFilterPredicate).ToArrayAsync(CancellationToken);
        }

        public async Task<TEntity> Get(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var fieldName = propertySelectorExpression.GetMember().Name;
            var entity = await ReadonlyQueryExecutor.SingleOrDefaultAsync(x => (string)x.GetType().GetProperty(fieldName).GetValue(typeof(TEntity)) == id);
            return entity;
        }

        public async Task<TEntity> Get(int id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var fieldName = propertySelectorExpression.GetMember().Name;
            var entity = await ReadonlyQueryExecutor.SingleOrDefaultAsync(x => (int)x.GetType().GetProperty(fieldName).GetValue(typeof(TEntity)) == id);
            return entity;
        }

        public async Task<List<TEntity>> GetMany(string[] ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var fieldName = propertySelectorExpression.GetMember().Name;
            return await ReadonlyQueryExecutor
                .Where(x => ids.Contains((string)x.GetType().GetProperty(fieldName).GetValue(typeof(TEntity))))
                .ToListAsync(CancellationToken);
        }

        public async Task<List<TEntity>> GetMany(List<string> ids, Expression<Func<ConversationNode, string>> propertySelectorExpression)
        {
            var fieldName = propertySelectorExpression.GetMember().Name;
            return await ReadonlyQueryExecutor
                .Where(x => ids.Contains((string)x.GetType().GetProperty(fieldName).GetValue(typeof(TEntity))))
                .ToListAsync(CancellationToken);
        }

        public Task<List<TEntity>> GetMany(int[] ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var fieldName = propertySelectorExpression.GetMember().Name;
            return ReadonlyQueryExecutor
                .Where(x => ids.Contains((int)x.GetType().GetProperty(fieldName).GetValue(typeof(TEntity))))
                .ToListAsync(CancellationToken);
        }

        public Task<List<TEntity>> GetMany(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var fieldName = propertySelectorExpression.GetMember().Name;
            return ReadonlyQueryExecutor
                .Where(x => id == (string)x.GetType().GetProperty(fieldName).GetValue(typeof(TEntity)))
                .ToListAsync(CancellationToken);
        }

        public Task<List<TEntity>> GetMany(int id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var fieldName = propertySelectorExpression.GetMember().Name;
            return ReadonlyQueryExecutor
                .Where(x => id == (int)x.GetType().GetProperty(fieldName).GetValue(typeof(TEntity)))
                .ToListAsync(CancellationToken);
        }


        public async Task<TEntity[]> GetAll()
        {
            var entities = await ReadonlyQueryExecutor.ToListAsync(CancellationToken);
            return entities.ToArray();
        }

        public async Task<TEntity> GetDeep(string id, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var fieldName = propertySelectorExpression.GetMember().Name;
            var transientQueryable = ChooseContext(contextProvider)
                .Set<TEntity>()
                .AsNoTracking()
                .Where(x => x.AccountId == AccountId)
                .Where(x => id == (string)x.GetType().GetProperty(fieldName).GetValue(typeof(TEntity)))
                .Include(ChooseContext(contextProvider).GetIncludePaths(typeof(TEntity)));

            return await transientQueryable.SingleAsync(CancellationToken);
        }


        public async Task<TEntity[]> GetAllDeep()
        {
            var transientQueryable = ChooseContext(contextProvider)
                .Set<TEntity>()
                .AsNoTracking()
                .Where(x => x.AccountId == AccountId)
                .Include(ChooseContext(contextProvider).GetIncludePaths(typeof(TEntity)));
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
            var fieldName = propertySelectorExpression.GetMember().Name;
            var entities = await QueryExecutor
                .Where(x => id == (string)x.GetType().GetProperty(fieldName).GetValue(typeof(TEntity)))
                .ToListAsync(CancellationToken);
            QueryExecutor.RemoveRange(entities);
        }

        public async Task Delete(TEntity[] entities)
        {
            await Task.CompletedTask;
            AssertAccountIsCorrect(entities.First());
            QueryExecutor.RemoveRange(entities);
        }

        public async Task Delete(string[] ids, Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            var entities = await GetMany(ids, propertySelectorExpression);
            QueryExecutor.RemoveRange(entities);
        }

        public IQueryable<TEntity> Query()
        {
            return QueryExecutor.Where(x => x.AccountId == AccountIdTransport.AccountId);
        }
    }
}