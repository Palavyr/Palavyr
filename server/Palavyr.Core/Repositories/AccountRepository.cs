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
        private readonly ITransportACancellationToken ctTransport;
        public IHoldAnAccountId AccountIdHolder { get; private set; }

        public AccountRepository(AccountsContext accountsContext, ILogger<AccountRepository> logger, IRemoveStaleSessions removeStaleSessions, IGuidUtils guidUtils, IHoldAnAccountId accountIdHolder, ITransportACancellationToken cancellationToken)
        {
            this.accountsContext = accountsContext;
            this.logger = logger;
            this.removeStaleSessions = removeStaleSessions;
            this.guidUtils = guidUtils;
            this.ctTransport = cancellationToken;
            AccountIdHolder = accountIdHolder;
            
        }

        public async Task CommitChangesAsync()
        {
            await accountsContext.SaveChangesAsync(ctTransport.CancellationToken);
        }

        public async Task<Account> GetAccount()
        {
            logger.LogInformation($"Retrieving user account: {AccountIdHolder.AccountId}");
            return await accountsContext.Accounts.SingleAsync(row => row.AccountId == AccountIdHolder.AccountId, ctTransport.CancellationToken);
        }

        public async Task<Account> GetAccountOrNull()
        {
            logger.LogInformation($"Retrieving user account: {AccountIdHolder.AccountId}");
            return await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == AccountIdHolder.AccountId, ctTransport.CancellationToken);
        }

        public async Task<Account?> GetAccountByEmailOrNull(string emailAddress)
        {
            logger.LogInformation($"Retrieving user account: {emailAddress}");
            return await accountsContext.Accounts.SingleOrDefaultAsync(row => row.EmailAddress == emailAddress, ctTransport.CancellationToken);
        }

        public async Task<Account> GetAccountByEmailAddressOrNull(string emailAddress)
        {
            logger.LogInformation($"Retrieving user account by email: {emailAddress}");
            return await accountsContext.Accounts.SingleOrDefaultAsync(row => row.EmailAddress == emailAddress, ctTransport.CancellationToken);
        }

        public async Task<Session> CreateAndAddNewSession(string token, string apiKey)
        {
            await removeStaleSessions.CleanSessionDb();
            var session = Session.CreateNew(token, AccountIdHolder.AccountId, apiKey);
            var newSession = await accountsContext.Sessions.AddAsync(session, ctTransport.CancellationToken);
            await accountsContext.SaveChangesAsync(ctTransport.CancellationToken);
            return newSession.Entity;
        }

        public async Task<Session> CreateAndAddNewSession(Account account)
        {
            var token = guidUtils.CreateNewId();
            var session = Session.CreateNew(token, account.AccountId, account.ApiKey);
            var newSession = await accountsContext.Sessions.AddAsync(session, ctTransport.CancellationToken);
            await accountsContext.SaveChangesAsync(ctTransport.CancellationToken);
            return newSession.Entity;
        }

        public async Task<Session?> GetSessionOrNull(string token)
        {
            var session = await accountsContext.Sessions.SingleOrDefaultAsync(row => row.SessionId == token, ctTransport.CancellationToken);
            return session;
        }

        public async Task RemoveSession(string sessionId)
        {
            var session = await accountsContext
                .Sessions
                .SingleOrDefaultAsync(row => row.SessionId == sessionId, ctTransport.CancellationToken);

            if (session != null)
            {
                accountsContext.Sessions.Remove(session);
            }
        }

        public async Task<bool> SignedStripePayloadExists(string signedPayload)
        {
            var previousRecords = await accountsContext.StripeWebHookRecords.Where(row => row.PayloadSignature == signedPayload).ToArrayAsync(ctTransport.CancellationToken);
            return previousRecords.Length > 0;
        }
    }
}