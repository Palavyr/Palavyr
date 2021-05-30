using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Repositories.Delete
{
    public class AccountDeleter : AccountRepository, IAccountDeleter
    {
        private readonly AccountsContext accountsContext;
        private readonly StripeCustomerService stripeCustomerService;
        private readonly ILogger<AccountDeleter> logger;

        public AccountDeleter(
            AccountsContext accountsContext,
            StripeCustomerService stripeCustomerService,
            IRemoveStaleSessions removeStaleSessions,
            ILogger<AccountDeleter> logger,
            IGuidUtils guidUtils
        )
            : base(accountsContext, logger, removeStaleSessions, guidUtils)
        {
            this.accountsContext = accountsContext;
            this.stripeCustomerService = stripeCustomerService;
            this.logger = logger;
        }

        public async Task DeleteAccount(string accountId, CancellationToken cancellationToken)
        {
            await DeleteAccountRecord(accountId, cancellationToken);
            DeleteEmailVerifications(accountId);
            DeleteSessionsByAccount(accountId);
            DeleteSubscriptionsByAccount(accountId);
            await DeleteStripeIntegration(accountId, cancellationToken);
        }

        public async Task DeleteStripeIntegration(string accountId, CancellationToken cancellationToken)
        {
            var account = await GetAccount(accountId, cancellationToken);
            if (!string.IsNullOrEmpty(account.StripeCustomerId))
            {
                await stripeCustomerService.DeleteSingleLiveStripeCustomer(account.StripeCustomerId);
            }
        }

        public async Task DeleteAccountRecord(string accountId, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Deleting the account record for {accountId}");
            var account = await GetAccount(accountId, cancellationToken);
            accountsContext.Accounts.Remove(account);
        }

        public void DeleteEmailVerifications(string accountId)
        {
            logger.LogInformation($"Deleting email verifications from {accountId}");
            var emailVerifications = accountsContext.EmailVerifications.Where(row => row.AccountId == accountId);
            accountsContext.EmailVerifications.RemoveRange(emailVerifications);
        }

        public void DeleteSessionsByAccount(string accountId)
        {
            logger.LogInformation($"Deleting active sessions for {accountId}");
            var sessions = accountsContext.Sessions.Where(row => row.AccountId == accountId);
            accountsContext.Sessions.RemoveRange(sessions);
        }

        public void DeleteSubscriptionsByAccount(string accountId)
        {
            logger.LogInformation($"Deleting subscriptions for {accountId}");
            var subs = accountsContext.Subscriptions.Where(row => row.AccountId == accountId);
            accountsContext.Subscriptions.RemoveRange(subs);
        }
    }
}