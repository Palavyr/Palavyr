using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class StripeWebhookAccountGetter : IStripeWebhookAccountGetter
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly IAccountIdTransport accountIdTransport;

        public StripeWebhookAccountGetter(IEntityStore<Account> accountStore, IAccountIdTransport accountIdTransport)
        {
            this.accountStore = accountStore;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<Account> GetAccount(string stripeCustomerId)
        {
            var account = await accountStore
                .DangerousRawQuery()
                .SingleOrDefaultAsync(x => x.StripeCustomerId == stripeCustomerId);
            if (account is null)
            {
                throw new DomainException("Failed to find the account using the customer Id");
            }

            if (accountIdTransport.IsSet())
            {
                throw new Exception("The account has already been set. This indicates a crossing of wires with the account separation system!");
            }

            accountIdTransport.Assign(account.AccountId);

            return account;
        }
    }
}