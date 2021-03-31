using System.Threading.Tasks;

namespace Palavyr.Core.Repositories.Delete
{
    public interface IAccountDeleter : IAccountRepository
    {
        Task DeleteAccount(string accountId);
        Task DeleteAccountRecord(string accountId);
        void DeleteEmailVerifications(string accountId);
        void DeleteSessionsByAccount(string accountId);
        void DeleteSubscriptionsByAccount(string accountId);
    }
}