using System.Linq;
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
        private readonly IAccountIdTransport accountIdTransport;

        public AccountDeleter(
            AccountsContext accountsContext,
            StripeCustomerService stripeCustomerService,
            IRemoveStaleSessions removeStaleSessions,
            ILogger<AccountDeleter> logger,
            IGuidUtils guidUtils,
            IAccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport
        )
            : base(accountsContext, logger, removeStaleSessions, guidUtils, accountIdTransport, cancellationTokenTransport)
        {
            this.accountsContext = accountsContext;
            this.stripeCustomerService = stripeCustomerService;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task DeleteAccount()
        {
            await DeleteAccountRecord();
            DeleteEmailVerifications();
            DeleteSessionsByAccount();
            DeleteSubscriptionsByAccount();
            await DeleteStripeIntegration();
        }

        public async Task DeleteStripeIntegration()
        {
            var account = await GetAccount();
            if (!string.IsNullOrEmpty(account.StripeCustomerId))
            {
                await stripeCustomerService.DeleteSingleLiveStripeCustomer(account.StripeCustomerId);
            }
        }

        public async Task DeleteAccountRecord()
        {
            logger.LogInformation($"Deleting the account record for {accountIdTransport.AccountId}");
            var account = await GetAccount();
            accountsContext.Accounts.Remove(account);
        }

        public void DeleteEmailVerifications()
        {
            logger.LogInformation($"Deleting email verifications from {accountIdTransport.AccountId}");
            var emailVerifications = accountsContext.EmailVerifications.Where(row => row.AccountId == accountIdTransport.AccountId);
            accountsContext.EmailVerifications.RemoveRange(emailVerifications);
        }

        public void DeleteSessionsByAccount()
        {
            logger.LogInformation($"Deleting active sessions for {accountIdTransport.AccountId}");
            var sessions = accountsContext.Sessions.Where(row => row.AccountId == accountIdTransport.AccountId);
            accountsContext.Sessions.RemoveRange(sessions);
        }

        public void DeleteSubscriptionsByAccount()
        {
            logger.LogInformation($"Deleting subscriptions for {accountIdTransport.AccountId}");
            var subs = accountsContext.Subscriptions.Where(row => row.AccountId == accountIdTransport.AccountId);
            accountsContext.Subscriptions.RemoveRange(subs);
        }
    }
}