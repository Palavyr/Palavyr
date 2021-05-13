#nullable enable
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UIDUtils;
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

        public AccountRepository(AccountsContext accountsContext, ILogger<AccountRepository> logger, IRemoveStaleSessions removeStaleSessions)
        {
            this.accountsContext = accountsContext;
            this.logger = logger;
            this.removeStaleSessions = removeStaleSessions;
        }

        public async Task CommitChangesAsync()
        {
            await accountsContext.SaveChangesAsync();
        }

        public async Task<Account> GetAccount(string accountId, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Retrieving user account: {accountId}");
            return await accountsContext.Accounts.SingleAsync(row => row.AccountId == accountId, cancellationToken);
        }

        public async Task<Account> GetAccountOrNull(string accountId)
        {
            logger.LogInformation($"Retrieving user account: {accountId}");
            return await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
        }

        public async Task<Account?> GetAccountByEmailOrNull(string emailAddress)
        {
            logger.LogInformation($"Retrieving user account: {emailAddress}");
            return await accountsContext.Accounts.SingleOrDefaultAsync(row => row.EmailAddress == emailAddress);
        }

        public async Task<Account> GetAccountByEmailAddressOrNull(string emailAddress)
        {
            logger.LogInformation($"Retrieving user account by email: {emailAddress}");
            return await accountsContext.Accounts.SingleOrDefaultAsync(row => row.EmailAddress == emailAddress);
        }

        public async Task<Session> CreateAndAddNewSession(string token, string accountId, string apiKey)
        {
            await removeStaleSessions.CleanSessionDb();
            var session = Session.CreateNew(token, accountId, apiKey);
            var newSession = await accountsContext.Sessions.AddAsync(session);
            await accountsContext.SaveChangesAsync();
            return newSession.Entity;
        }

        public async Task<Session> CreateAndAddNewSession(Account account)
        {
            var token = GuidUtils.CreateNewId();
            var session = Session.CreateNew(token, account.AccountId, account.ApiKey);
            var newSession = await accountsContext.Sessions.AddAsync(session);
            await accountsContext.SaveChangesAsync();
            return newSession.Entity;
        }

        public async Task<Session?> GetSessionOrNull(string token)
        {
            var session = await accountsContext.Sessions.SingleOrDefaultAsync(row => row.SessionId == token);
            return session;
        }

        public async Task RemoveSession(string sessionId)
        {
            var session = await accountsContext
                .Sessions
                .SingleOrDefaultAsync(row => row.SessionId == sessionId);

            if (session != null)
            {
                accountsContext.Sessions.Remove(session);
            }
        }

        public bool SignedStripePayloadExists(string signedPayload)
        {
            var previousRecords = accountsContext.StripeWebHookRecords.Where(row => row.PayloadSignature == signedPayload).ToArray();
            return previousRecords.Length > 0;
        }
    }
}