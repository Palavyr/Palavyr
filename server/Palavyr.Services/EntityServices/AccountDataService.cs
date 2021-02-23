using System.Linq;
using Palavyr.Data;
using Palavyr.Domain.Accounts.Schemas;

namespace Palavyr.Services.EntityServices
{
    public interface IAccountDataService
    {
        Account GetUserAccount(string accountId);
    }

    public class AccountDataService : IAccountDataService
    {
        private readonly AccountsContext accountsContext;

        public AccountDataService(AccountsContext accountsContext)
        {
            this.accountsContext = accountsContext;
        }
        
        public Account GetUserAccount(string accountId)
        {
            var userAccount = accountsContext.Accounts.Single(row => row.AccountId == accountId);
            return userAccount;
        }
    }
}