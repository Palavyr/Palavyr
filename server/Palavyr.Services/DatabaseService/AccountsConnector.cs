#nullable enable
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Common.UIDUtils;
using Palavyr.Data;
using Palavyr.Domain.Accounts.Schemas;

namespace Palavyr.Services.DatabaseService
{
    public interface IAccountsConnector
    {
        Task CommitChangesAsync();
        Task<Account> GetAccount(string accountId);
        Task<Account?> GetAccountOrNull(string accountId);
        Task<Account?> GetAccountByEmailOrNull(string emailAddress);
        Task<Account?> GetAccountByEmailAddressOrNull(string emailAddress);
        Task<Session> CreateAndAddNewSession(string token, string accountId, string apiKey);
        Task<Session> CreateAndAddNewSession(Account account);
        Task<Session?> GetSessionOrNull(string token);
        Task RemoveSession(string sessionId);
        bool SignedStripePayloadExists(string signedPayload);
    }

    public class AccountsConnector : IAccountsConnector
    {
        private readonly AccountsContext accountsContext;
        private readonly ILogger<AccountsConnector> logger;

        public AccountsConnector(AccountsContext accountsContext, ILogger<AccountsConnector> logger)
        {
            this.accountsContext = accountsContext;
            this.logger = logger;
        }

        public async Task CommitChangesAsync()
        {
            await accountsContext.SaveChangesAsync();
        }

        public async Task<Account> GetAccount(string accountId)
        {
            logger.LogInformation($"Retrieving user account: {accountId}");
            return await accountsContext.Accounts.SingleAsync(row => row.AccountId == accountId);
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