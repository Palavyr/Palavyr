#nullable enable
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountsContext accountsContext;
        private readonly ILogger<AccountRepository> logger;
        private readonly IRemoveStaleSessions removeStaleSessions;
        private readonly IGuidUtils guidUtils;
        private readonly ICancellationTokenTransport ctCancellationTokenTransport;
        public IAccountIdTransport AccountIdTransport { get; private set; }

        public AccountRepository(
            AccountsContext accountsContext,
            ILogger<AccountRepository> logger,
            IRemoveStaleSessions removeStaleSessions,
            IGuidUtils guidUtils,
            IAccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport)
        {
            this.accountsContext = accountsContext;
            this.logger = logger;
            this.removeStaleSessions = removeStaleSessions;
            this.guidUtils = guidUtils;
            ctCancellationTokenTransport = cancellationTokenTransport;
            AccountIdTransport = accountIdTransport;
        }

        public async Task CommitChangesAsync()
        {
            await accountsContext.SaveChangesAsync(ctCancellationTokenTransport.CancellationToken);
        }

        public async Task<Account> GetAccount()
        {
            logger.LogInformation($"Retrieving user account: {AccountIdTransport.AccountId}");
            return await accountsContext.Accounts.SingleAsync(row => row.AccountId == AccountIdTransport.AccountId, ctCancellationTokenTransport.CancellationToken);
        }

        public async Task<Account> GetAccountOrNull()
        {
            logger.LogInformation($"Retrieving user account: {AccountIdTransport.AccountId}");
            return await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == AccountIdTransport.AccountId, ctCancellationTokenTransport.CancellationToken);
        }

        public async Task<Account?> GetAccountByEmailOrNull(string emailAddress)
        {
            logger.LogInformation($"Retrieving user account: {emailAddress}");
            return await accountsContext.Accounts.SingleOrDefaultAsync(row => row.EmailAddress == emailAddress, ctCancellationTokenTransport.CancellationToken);
        }

        public async Task<Account?> GetAccountByEmailAddressOrNull(string emailAddress)
        {
            logger.LogInformation($"Retrieving user account by email: {emailAddress}");
            return await accountsContext.Accounts.SingleOrDefaultAsync(row => row.EmailAddress == emailAddress, ctCancellationTokenTransport.CancellationToken);
        }

        public async Task<Session> CreateAndAddNewSession(string token, string apiKey)
        {
            await removeStaleSessions.CleanSessionDb();
            var session = Session.CreateNew(token, AccountIdTransport.AccountId, apiKey);
            return await CreateNewSession(session);
        }

        public async Task<Session> CreateNewSession(Session session)
        {
            var newSession = await accountsContext.Sessions.AddAsync(session, ctCancellationTokenTransport.CancellationToken);
            await accountsContext.SaveChangesAsync(ctCancellationTokenTransport.CancellationToken);
            return newSession.Entity;
        }

        public async Task<Session> CreateAndAddNewSession(Account account)
        {
            var token = guidUtils.CreateNewId();
            var session = Session.CreateNew(token, account.AccountId, account.ApiKey);
            var newSession = await accountsContext.Sessions.AddAsync(session, ctCancellationTokenTransport.CancellationToken);
            await accountsContext.SaveChangesAsync(ctCancellationTokenTransport.CancellationToken);
            return newSession.Entity;
        }

        public async Task<Session?> GetSessionOrNull(string token)
        {
            var session = await accountsContext.Sessions.SingleOrDefaultAsync(row => row.SessionId == token, ctCancellationTokenTransport.CancellationToken);
            return session;
        }

        public async Task RemoveSession(string sessionId)
        {
            var session = await accountsContext
                .Sessions
                .SingleOrDefaultAsync(row => row.SessionId == sessionId, ctCancellationTokenTransport.CancellationToken);

            if (session != null)
            {
                accountsContext.Sessions.Remove(session);
            }
        }

        public async Task<bool> SignedStripePayloadExists(string signature)
        {
            var previousRecords = await accountsContext.StripeWebHookRecords.Where(row => row.PayloadSignature == signature).ToArrayAsync(ctCancellationTokenTransport.CancellationToken);
            return previousRecords.Length > 0;
        }

        public async Task AddStripeEvent(string id, string signature)
        {
            var newRecord = StripeWebhookRecord.CreateNewRecord(id, signature);
            await accountsContext.StripeWebHookRecords.AddAsync(newRecord);
            await accountsContext.SaveChangesAsync(ctCancellationTokenTransport.CancellationToken);
        }

        public async Task<Account> CreateAccount(Account account)
        {
            var entity = await accountsContext.Accounts.AddAsync(account);
            await accountsContext.SaveChangesAsync(ctCancellationTokenTransport.CancellationToken);
            return entity.Entity;
        }
    }
}