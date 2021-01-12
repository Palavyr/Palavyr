using System.Linq;
using DashboardServer.Data;
using Server.Domain.Accounts;

namespace Palavyr.API.Services.EntityServices
{
    public interface IAccountDataService
    {
        UserAccount GetUserAccount(string accountId);
    }

    public class AccountDataService : IAccountDataService
    {
        private readonly AccountsContext accountsContext;

        public AccountDataService(AccountsContext accountsContext)
        {
            this.accountsContext = accountsContext;
        }
        
        public UserAccount GetUserAccount(string accountId)
        {
            var userAccount = accountsContext.Accounts.Single(row => row.AccountId == accountId);
            return userAccount;
        }
    }
}