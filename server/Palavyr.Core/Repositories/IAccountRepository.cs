using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Repositories
{
    public interface IAccountRepository
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
}