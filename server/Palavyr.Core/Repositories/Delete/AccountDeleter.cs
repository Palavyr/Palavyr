using System;
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
        private readonly IHoldAnAccountId accountIdHolder;
        private readonly CancellationTokenTransport ctTransportTransport;

        public AccountDeleter(
            AccountsContext accountsContext,
            StripeCustomerService stripeCustomerService,
            IRemoveStaleSessions removeStaleSessions,
            ILogger<AccountDeleter> logger,
            IGuidUtils guidUtils,
            IHoldAnAccountId accountIdHolder,
            CancellationTokenTransport ctTransportTransport
        )
            : base(accountsContext, logger, removeStaleSessions, guidUtils, accountIdHolder, ctTransportTransport)
        {
            this.accountsContext = accountsContext;
            this.stripeCustomerService = stripeCustomerService;
            this.logger = logger;
            this.accountIdHolder = accountIdHolder;
            this.ctTransportTransport = ctTransportTransport;
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
            logger.LogInformation($"Deleting the account record for {accountIdHolder.AccountId}");
            var account = await GetAccount();
            accountsContext.Accounts.Remove(account);
        }

        public void DeleteEmailVerifications()
        {
            logger.LogInformation($"Deleting email verifications from {accountIdHolder.AccountId}");
            var emailVerifications = accountsContext.EmailVerifications.Where(row => row.AccountId == accountIdHolder.AccountId);
            accountsContext.EmailVerifications.RemoveRange(emailVerifications);
        }

        public void DeleteSessionsByAccount()
        {
            logger.LogInformation($"Deleting active sessions for {accountIdHolder.AccountId}");
            var sessions = accountsContext.Sessions.Where(row => row.AccountId == accountIdHolder.AccountId);
            accountsContext.Sessions.RemoveRange(sessions);
        }

        public void DeleteSubscriptionsByAccount()
        {
            logger.LogInformation($"Deleting subscriptions for {accountIdHolder.AccountId}");
            var subs = accountsContext.Subscriptions.Where(row => row.AccountId == accountIdHolder.AccountId);
            accountsContext.Subscriptions.RemoveRange(subs);
        }
    }
}